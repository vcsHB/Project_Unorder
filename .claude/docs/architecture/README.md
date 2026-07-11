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
| GameSystem | `GameSystem/` | Flow(챕터/컷/진행), Rule |
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

- 구현 매니저: `PlayerManager`, `CameraManager`, `CombatManager`, `RuleManager`(GameSystem/RuleSystem 둘 다), `VolumeManager`
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

### 4. Boss — Behavior Tree + AttackPattern(SO)
[Agent/BossSystem/Boss.cs](../../../Assets/01.Scripts/Agent/BossSystem/Boss.cs) 는 Unity `Unity.Behavior`(BehaviorGraphAgent)로 구동.
- `BossEncounterController` 가 페이즈 관리, 블랙보드 변수는 `GetVariable/SetVariable`.
- 공격 패턴은 `BossAttackPatternSO` 파생 ScriptableObject(코루틴 `Execute(context)`): `AimedPatternSO`, `RingPatternSO`, `SpreadPatternSO`, `WavePatternSO`.
- BT 노드: `CommonBT/Action`, `CommonBT/Condition`.

### 5. Flow — 챕터/컷 진행
`GameSystem/Flow/`. `FlowManager` → `ChapterGroupData` → `ChapterData` → `FlowStep` 인스턴스화 후 `StartFlow()`. 연출 단위는 `FlowCue` 파생(Camera/Dialogue/Music/Volume/Condition 등).

### 6. Rule — 위반 감지
`RuleSystem/`. `RuleObserver`(static)가 규칙별 이벤트(`RULE_BoxLimit`, `RULE_LifeTime`)를 `Trigger(isViolated)`로 발행 → `RuleManager`가 구독 → `IPenaltiable`로 패널티. 흐름: **Observer → Manager → Penalty**.

### 7. ObjectPooling
`ObjectPooling/`. `PoolManager` + `PoolGroupSO`, 풀 키는 `Generated/PoolTablePoolKey.cs`로 코드 생성. 재사용 오브젝트는 `IPoolable`.

## 규약 요약 (상세: [conventions.md](../../conventions.md))
- 클래스/메서드 PascalCase, 지역/필드 camelCase, private 필드 `_camelCase`, 상수/Enum PascalCase.
- Update 루프 내 `new`·LINQ·foreach 지양(성능). 필수 컴포넌트는 `[RequireComponent]`.
- 주석 없이 논리 구조만으로 의도가 드러나게 작성.
