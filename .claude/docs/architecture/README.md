# Architecture — 코드 구조 정리

> 코드 구조/시스템 지도. 새 매니저·시스템 추가 시 이 문서를 갱신한다.
> 루트 네임스페이스: `Project_Unorder.*` (일부 유틸은 `MINISoundManage`, `ObjectPooling`, `DataManage` 등 예외)
> 스크립트 루트: `Assets/01.Scripts/`

## 시스템 지도

| 시스템 | 경로 | 역할 |
|---|---|---|
| Core | `Core/` | 싱글턴, 매니저 초기화, 커스텀 어트리뷰트, 에디터 유틸 |
| Agent | `Agent/` | 인게임 엔티티 베이스 + 컴포넌트 시스템 (Player / Enemy / Boss) |
| CombatSystem | `CombatSystem/` | 데미지, Caster(범위판정), Projectile(탄막), HitBody |
| GameSystem | `GameSystem/` | Flow(챕터/컷/진행), Boot(부팅 진입점), Rule |
| Boot | `GameSystem/Boot/` | `BootProfile` 기반 단일 진입점 — 전투 단독 실행 / 특정 Flow 시점 시작 |
| RuleSystem | `RuleSystem/` | 규칙 위반 감지(Observer) → 패널티 |
| UI | `UI/` | 인게임 UI, 패널, 페이지, 로그 뷰 |
| SoundSystem | `SoundSystem/` | 사운드 풀/테이블 (`MINISoundManage`) |
| ObjectPooling | `ObjectPooling/` | 범용 풀 매니저 + 코드젠 풀 키 |
| PhysicsSystem | `PhysicsSystem/` | 커스텀 물리 바디, 넉백/중력 인터페이스 |
| DialogueSystem | `DialogueSystem/` | 대사 + 텍스트 애니메이션 |
| LogSystem | `LogSystem/` | 인게임 로그 채널/시퀀스/UI |
| NodeSystem | `NodeSystem/` | 노드 그래프 데이터 |
| CameraSystem | `CameraSystem/` | 카메라 매니저 + 컴포넌트 |
| VolumeManage | `VolumeManage/` | URP 포스트프로세스 볼륨 제어 |
| FeedbackSystem | `FeedbackSystem/` | 연출 피드백 재생 |
| Object | `Object/` | 상자, 전투 오브젝트, VFX |
| InfomationSystem | `InfomationSystem/` | 정보 데이터(그룹) |
| ActionSystem | `ActionSystem/` | 액션 매니저 |
| DataManage | `DataManage/` | JSON 유틸(MiniJson) |

## 핵심 패턴

### 1. 매니저 초기화 순서 — `IEarlyAwakeableManager`
[Core/ManagerInitializer.cs](../../../Assets/01.Scripts/Core/ManagerInitializer.cs) 가 씬의 자식 매니저 중 `IEarlyAwakeableManager` 구현체를 모아 Unity `Awake`보다 **먼저** `PreAwake()`를 호출한다.

- 구현 매니저: `PlayerManager`, `CameraManager`, `CombatManager`, `RuleManager`(`RuleSystem/`), `VolumeManager`
- 순서 의존 초기화는 `Awake`가 아니라 `PreAwake()`에 둘 것.

### 2. 싱글턴 — `MonoSingleton<T>`
[Core/MonoSingleton.cs](../../../Assets/01.Scripts/Core/MonoSingleton.cs). `Instance` 접근 시 없으면 `FindAnyObjectByType` → 그래도 없으면 `new GameObject().AddComponent<T>()`. 훅: `OnCreatedInstance()`.
- 사용처: `PlayerManager`, `CameraManager`, `SoundController`, `VolumeManager`

### 3. Agent 컴포넌트 시스템
[Agent/Agent.cs](../../../Assets/01.Scripts/Agent/Agent.cs) 가 인게임 엔티티 베이스.
- 자식의 `IAgentComponent`를 타입별 딕셔너리로 수집.
- 초기화 순서: `Initialize(agent)` → `AfterInitialize()` → `LateInitialize()`(Start), 파괴 시 `Dispose`.
- 조회: `GetCompo<T>(isDerived)` — 딕셔너리 미스 시 자식에서 찾아 캐시, `isDerived`면 하위 클래스도 매칭.
- 파생: `Boss : Agent`, `Enemy`, `Player`.

### 4. Boss — 패턴 러너 + 전략 패턴(SO)
> 2026-09-18 Behavior Tree(`Unity.Behavior`) 폐기. 근거: 그래프가 빈 상태였고, 흐름이 "선택 → 실행 → 대기 → 페이즈 전환"뿐이라 코드가 더 단순하며, Rule·Turn이 끼어들 이벤트를 열기 쉽다.

[Agent/BossSystem/Boss.cs](../../../Assets/01.Scripts/Agent/BossSystem/Boss.cs)는 얇은 진입점이고, 실제 구동은 [BossPatternRunner](../../../Assets/01.Scripts/Agent/BossSystem/BossPatternRunner.cs)(`IAgentComponent`)가 맡는다.

| 역할 | 타입 | 전략 |
|---|---|---|
| 무엇을 쏘나 | `BossAttackPatternSO` → `AimedPatternSO`·`RingPatternSO`·`SpreadPatternSO`·`WavePatternSO` | `Execute(context)` 코루틴 |
| 다음에 뭘 쏘나 | `PatternSelectorSO` → `Sequential`·`Random`(연속 중복 회피)·`Weighted` | `Select(entries, state)` |
| 페이즈 구성 | `BossPhaseDataSO.Patterns[]`(`BossPatternEntry`: pattern·weight·delayAfter) + `Selector` + `NextPhaseHpThreshold` | 데이터 |

- 루프: 일시정지 대기 → HP 임계값이면 `BossEncounterController.RequestPhaseTransition` → 선택 → 실행 → `delayAfter` 대기.
- **선택 상태는 SO가 아니라 러너가 가진다**(`PatternSelectionState`). SO는 여러 보스가 공유하므로 상태를 두면 안 된다.
- `Selector`가 비어 있으면 순차로 동작한다.
- 이벤트(Rule·Turn 개입점): `OnPhaseEnterEvent(int)`, `OnPatternStartEvent(pattern)`, `OnPatternEndEvent(pattern)`, `Boss.OnPhaseChangedEvent(int)`.
- 테스트 API: `RequestPattern(pattern)`(다음 슬롯 강제), `SetPaused(bool)`, `StopRunning()`. 보스 사망(`HealthBody.OnDieEvent`) 시 자동 정지.
- 타깃은 `PlayerDataSO`(기본 Order)의 등록 인스턴스를 쓴다.
- `Boss`는 `[RequireComponent(HealthBody, BossPatternRunner)]`. 탄 발사는 자식의 `BossProjectileEmitter`, 탄 종류는 `ProjectileSO`이며 **`ProjectileManager._enabledProjectiles`에 등록돼야 풀이 생긴다.**

### 5. Flow — 챕터/컷 진행
`GameSystem/Flow/`. `FlowManager` → `ChapterGroupData` → `ChapterData` → `FlowStep` 인스턴스화. 연출 단위는 `FlowCue` 파생(Camera/Dialogue/Music/Volume/Condition 등).
- **시작 주체는 `FlowManager` 하나다.** `FlowStep.Awake()`는 큐 수집만 하고, 자동 시작은 `Start()`에서 `_autoStartOnSceneLoad`가 켜져 있을 때만 일어난다.
- `FlowManager`가 인스턴스화할 때는 `PrepareExternalStart(level)`로 자동 시작을 끄고 레벨을 주입한 뒤 명시적으로 시작한다 — **이중 시작 금지 계약**.
- `StartChapter(chapterId, step)`으로 임의 시점 진입, 스텝 종료 시 다음 스텝으로 자동 진행(`_advanceToNextStep`), 챕터 끝에서 `OnChapterEndEvent`.
- 진행 저장은 `CaptureSnapshot()` / `RestoreSnapshot(FlowData)` (`FlowSnapshot`).

### 6. Rule — 위반 감지
`RuleSystem/`. `RuleObserver`(static)가 규칙별 이벤트(`RULE_BoxLimit`)를 `Trigger(isViolated)`로 발행 → `RuleManager`가 구독 → `IPenaltiable`로 패널티. 흐름: **Observer → Manager → Penalty**.
> ⚠️ 현재 발행자·구독자·패널티 구현체가 모두 비어 있다(가동률 0%). 설계 제약은 [design/core-design.md §3.2](../design/core-design.md), 일정은 [milestones.md](../milestones.md) M2.

### 7. ObjectPooling
`ObjectPooling/`. `PoolManager` + `PoolGroupSO`, 풀 키는 `Generated/PoolTablePoolKey.cs`로 코드 생성. 재사용 오브젝트는 `IPoolable`.

### 8. Boot — 단일 진입점
`GameSystem/Boot/`. 게임이 **어떤 상태로 시작할지**를 `BootProfile`(SO) 하나로 표현하고, `GameBootstrapper`가 그에 맞춰 `FlowManager` 또는 `CombatManager`를 호출한다.

- `BootMode.Normal` — 아무것도 하지 않는다. 씬에 배치된 `FlowStep`이 스스로 시작(기존 동작 유지).
- `BootMode.CombatOnly` — Flow를 건너뛰고 `CombatManager.StartCombat()` → `BossEncounterController.StartEncounter(skipIntro, startPhaseIndex)`.
- `BootMode.FlowFrom` — `FlowManager.StartChapter(chapterId, step)`로 임의 시점부터 재생.
- `Normal`이 아닐 때 `GameBootstrapper.Awake()`가 씬의 `FlowStep`에 `CancelAutoStart()`를 걸어 씬 기본 동작을 덮어쓴다(`Awake` → 모든 `Start` 순서 보장 이용).
- **런타임 개념이다.** 에디터 창(T2)은 이 프로필을 채우는 UI일 뿐이며, `#if UNITY_EDITOR`가 런타임 로직에 들어가지 않는다.

## 규약 요약 (상세: [conventions.md](../../conventions.md))
- 클래스/메서드 PascalCase, 지역/필드 camelCase, private 필드 `_camelCase`, 상수/Enum PascalCase.
- Update 루프 내 `new`·LINQ·foreach 지양(성능). 필수 컴포넌트는 `[RequireComponent]`.
- 주석 없이 논리 구조만으로 의도가 드러나게 작성.
