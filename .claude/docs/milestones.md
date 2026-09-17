# Milestones — 개발 마일스톤

> 기준일: 2026-09-18 · 근거: 코드 실사(`Assets/01.Scripts/`) + [design/core-design.md](design/core-design.md)
> 갱신 트리거: **마일스톤 착수/완료**, **DoD 변경**, **기획 미결 항목 해소**.
> 구조 변경은 [architecture/](architecture/README.md), 기획 변경은 [design/](design/README.md)에 함께 반영한다.

---

## 0. 현재 상태 한 줄 요약

**주변 시스템(연출·데이터·풀링·UI 프레임)은 두텁게 깔려 있으나, 게임을 "게임"으로 성립시키는 코어 3종 — 턴제 / Rule / Action — 이 비어 있다.**
즉 지금은 *탄막을 쏘는 보스와 피하는 플레이어*까지는 되지만, **턴이 돌지 않고 룰이 발동하지 않는다.**

---

## 1. 구현 현황 스냅샷

### 🟢 동작 가능 (골격 이상 구현됨)
| 영역 | 근거 |
|---|---|
| 매니저 초기화 계약 | `Core/ManagerInitializer.cs`, `IEarlyAwakeableManager` |
| Agent 컴포넌트 시스템 | `Agent/Agent.cs` (Initialize → AfterInitialize → LateInitialize) |
| Order 이동/상태 | `OrderPlayer/FSM/` (Idle·Move·Ground), `PlayerMover` |
| 보스 전투 구동 | `Boss.cs` + `BossPatternRunner`(선택 전략 SO) + `BossEncounterController`(페이즈/인트로/대사/시네마틱) — 2026-09-18 BT 폐기 |
| 보스 탄막 패턴 | `BossAttackPatternSO` 파생 4종(Aimed/Ring/Spread/Wave) + `BossProjectileEmitter` |
| 탄막 본체/풀링 | `Projectile`, `StraightProjectile`, `SpearProjectile`, `ProjectileManager`, `PoolManager` |
| **드래그 선택 → 정지 → 공격탄 전환** | `Interactor` → `DragZone.Release()` → `ISelectable.Select()`(속도 0 캐시) → `ProjectileController.ChargeProjectile(count)` → 오빗 → `OrderPlayerAttacker.HandleAttack()` → `TryShoot` |
| 제한 영역 | `Object/Box/` (`MapBox` 사이징 연출, `BoxWall`, `BoxController`) |
| Flow(챕터/컷) | `FlowManager` → `ChapterData` → `FlowStep` → `FlowCue` 파생 9종 |
| 지원 시스템 | Dialogue, Log, Sound, Volume, Camera, Feedback, VFX(Slice/Burst), UI Window/Page 프레임 |

### 🟡 부분 구현 (연결 또는 정식화 필요)
| 항목 | 현재 | 부족한 것 |
|---|---|---|
| Point 자원 | **오빗 탄 개수**가 사실상 Point 역할 (`ProjectileController._orbitSlots`) | 정식 자원 개념 없음 — 상한/소멸/턴 리셋/코스트/UI 표시 전무 |
| 사망 처리 | `HealthBody.HandleDie()` → `OnDieEvent` | 게임오버/승리 흐름에 연결 안 됨. 조건이 `_currentHealth < 0` 이라 **0에서 죽지 않음** |
| 전투 진입 | `CombatManager.StartCombat()` 존재 | **빈 스텁**. `CombatScene` SO도 `condfig` 오타 |
| Flow 저장 | `FlowSnapshot`/`FlowData` 정의됨 | 저장/복원 호출부 없음, `FlowManager`에 `// TODO : Check Range` |
| 노드 맵 | `NodeSystem/`(Node·NodeData·NodeController), `NodeScene.unity` | **껍데기만** — 로그라이크 런 구조 미착수 |

### 🔴 미착수 (코어 공백)
| 항목 | 근거 |
|---|---|
| **턴제 진행** | `TurnManager` 부재. 보스 턴↔플레이어 턴 전환 코드 전무 |
| **Rule 시스템 실동작** | `RuleManager.PreAwake()`의 구독이 **전부 주석**, `RuleObserver.RULE_BoxLimit.Trigger()`를 **호출하는 코드가 0곳**, `IPenaltiable` **구현체 0개** → 실질 가동률 0% |
| **Action(플레이어 턴 행동)** | `ActionManager.Act()` = `// TODO`. `ActionType` 7종 정의만 존재, UI(`ActionWindowPage`)와 미연결 |
| **Unorder 본체** | `UnorderPlayer` 클래스 **내용 없음** — 체력 룰(§3.3)이 붙을 자리 |
| **게임오버/승리/런 루프** | 없음 |

---

## 2. 마일스톤

> 순서는 **의존성 기준**. M0는 게이트, M1→M2는 코어, M3에서 처음으로 "플레이 가능한 슬라이스"가 나온다.

### M0 — 기획 잠금 (Design Lock) 🚧 게이트
코드 없이 문서로 끝내는 단계. [core-design.md §8 미결 5항목](design/core-design.md)을 확정한다.

- 턴 전환 트리거 기준 (패턴 큐 소진 / 타이머 / HP 임계 중 택)
- Point 규칙 (흡수 1탄 = ?Point, 상한, 턴 넘김 시 소멸 여부, 행동별 코스트)
- Unorder 체력 룰 발동·해제 조건, 체력 수치
- 드래그 능력 모듈 범위 (감속 외 무엇을, 어디서 설정)
- Rule 개입점 훅 형태 (M2 설계 입력)

**DoD** — `design/rule-system.md` 신규 + `core-design.md §8` 미결 항목 0개.
**왜 먼저인가** — M1·M2가 이 5개 답을 그대로 자료구조로 옮기는 작업이라, 미정인 채 짜면 전부 되돌린다.

---

### M1 — 전투 코어 루프 (턴이 도는 게임)
| 산출물 | 내용 |
|---|---|
| `TurnManager` | 보스 턴 ↔ 플레이어 턴 상태기. `IEarlyAwakeableManager` 계약 준수(G5). 턴 전환을 **이벤트로 방송**(M2 룰이 붙을 훅) |
| Point 자원 정식화 | 오빗 탄 개수에 얹힌 암묵 자원을 분리. 축적/소비/상한/리셋 + `PlayerStatusPanel` 표시 |
| `ActionManager.Act()` 구현 | `ActionType` 7종 중 최소 `Attack`/`Destory`부터. `ActionWindowPage` 선택 → 실행 연결 |
| 전투 종료 처리 | `HealthBody` 사망 조건 `<= 0` 수정 → Order 사망 = 게임오버 / 보스 사망 = 승리 |
| `CombatManager.StartCombat()` | `CombatScene` SO 기반 전투 진입 실장 (+`condfig` 오타 정리) |

**DoD** — CombatScene에서 *보스 턴(패턴 회피/흡수) → 플레이어 턴(Point 소비 공격) → 반복 → 한쪽 사망 시 종료*가 **끊김 없이 순환**한다.
**의존** — M0.

---

### M2 — Rule 시스템 골격 ★ 최우선 설계 제약
프로젝트의 **위험 특성**(보스별 애드혹 변칙 룰이 무한히 늘어남)을 감당할 구조를 먼저 세운다.

| 산출물 | 내용 |
|---|---|
| 개입점(Hook) 표준 | 플레이어 상태·이동·체력·턴·탄막·보스에 **이벤트/인터페이스로 개입점 노출**. 직접 결합 금지 |
| `RuleManager` 실동작 | 주석 처리된 구독 복원 + 룰 등록/해제/중재(충돌 우선순위) |
| `RULE_BoxLimit` 실연결 | 현재 `Trigger()` 호출부가 **한 곳도 없음** → `BoxWall`/`MapBox` 이탈 감지에 연결, `IPenaltiable` 구현체 1개 이상 |
| 검증용 룰 3종 | ① 영역 제약 ② **Unorder 체력 부여**(`UnorderPlayer` 실장 포함) ③ **턴 제거**(M1 `TurnManager`에 개입) |

**DoD** — **코어 루프·기존 룰을 한 줄도 고치지 않고** 새 룰 1개를 추가해 동작시킬 수 있다. (신규 룰 추가 PR의 diff가 신규 파일 + 등록 1줄에 그치는지로 판정)
**의존** — M0(훅 형태), M1(턴 제거 룰이 `TurnManager`를 필요로 함).
**리스크** — 이 단계를 대충 넘기면 보스가 늘어날 때마다 코어를 찢게 된다. 마일스톤 중 **가장 되돌리기 비싼 지점**.

---

### M3 — 오프닝 튜토리얼 수직 슬라이스 🎯 첫 플레이 가능 빌드
[core-design.md §6](design/core-design.md) 전개를 Flow 큐로 완주시킨다.

1. Order 자유 이동 체험 → 2. 최초 제약 발동(영역 학습) → 3. 보스 등장·패턴 회피 → 4. **Unorder 해금** → 본격 전투

| 산출물 | 내용 |
|---|---|
| Chapter_0 Flow 구성 | 기존 `FlowCue` 9종 조합 + 부족분(플레이어 해금 큐 등) 추가 |
| 씬 연결 | TitleScene → CombatScene 진입 경로 실장 |
| `FlowManager` 정리 | `// TODO : Check Range` 해소, 챕터 범위 검증 |

**DoD** — 빌드에서 **타이틀부터 첫 보스 전투까지 조작만으로 도달**하고, 룰 학습 지점(영역 제약)이 의도대로 전달된다.
**의존** — M1, M2.

---

### M4 — 보스 1체 완성
| 산출물 | 내용 |
|---|---|
| 페이즈 전환 완결 | `BossEncounterController` 전 페이즈 + 전환 연출·대사·시네마틱 |
| 패턴 셋 | 4종 SO를 페이즈별로 튜닝, 필요 시 패턴 추가 |
| 보스 전용 룰 | M2 훅 위에 **이 보스에만 붙는 변칙 룰** 최소 1개 (M2 확장성 실전 검증) |
| 사망/보상 연출 | 보스 사망 → 전투 종료 → 다음 흐름 |

**DoD** — 한 보스를 **처음부터 끝까지** 싸워 이길 수 있고, 그 과정에서 M2 구조를 수정할 필요가 없었다.
**의존** — M3.

---

### M5 — 로그라이크 런 구조
| 산출물 | 내용 |
|---|---|
| 노드 맵 | `NodeSystem/` 실장(`NodeController`/`Node` 껍데기 채우기), NodeScene 연결 |
| 런 진행/반복 | 런 시작 → 노드 선택 → 전투 → 보상 → 반복 → 사망 시 런 종료 |
| 세이브 | `FlowSnapshot` + `MiniJson` 저장/복원 |
| 메타 진행 | 런 간 유지되는 해금 요소(드래그 능력 모듈 등 — M0 4번 항목의 귀결) |

**DoD** — 런을 시작해 여러 노드를 거쳐 사망 또는 클리어로 끝나고, 재시작 시 메타 진행이 유지된다.
**의존** — M4.

---

### M6 — 폴리시 · 최적화 · 빌드
- GC 스파이크 점검(가드 G3 위반 지점 스윕 — `ProjectileController.UpdateOrbitProjectiles()`의 `foreach` 등)
- 탄막 대량 상황 프로파일링, 풀 크기 튜닝
- 사운드/현지화(`12.Localization`) 커버리지, 옵션·일시정지 완결
- 빌드 파이프라인 · 테스트(`run_tests`) 정비

---
### T트랙 — 테스트 툴킷 (M1·M2와 **병행** 필수)
> 이유: TurnManager·Rule을 짜면서 매번 *타이틀 → 튜토리얼 → 보스 등장*을 거쳐야 하면 반복 검증 비용이 개발 비용을 넘어선다.
> 특히 M2는 보스별 애드혹 룰을 **계속 붙여보며** 확장성을 검증해야 하므로, 툴 없이는 DoD 판정 자체가 불가능하다.

#### T0 — 부팅 진입점 단일화 ✅ 2026-09-18
툴을 만들기 전에 **시작 지점을 하나로 모으는 것**이 먼저였다. 조사해보니 실제 부팅 경로는 문서 초안의 추정과 달랐다:

| 실측된 현실 | 확인 방법 |
|---|---|
| `FlowStep_0_0_BEGIN.prefab`이 **CombatScene에 인스턴스로 배치**되어 `Awake()`로 자가 시작 — 이것이 현재 유일한 실제 부팅 경로 | 씬의 prefab guid 참조 |
| `FlowManager`는 씬에 있으나 **호출하는 코드가 0곳** | 전체 grep |
| `CombatManager`·`BossEncounterController`는 **어느 씬/프리팹에도 배치되어 있지 않음** | 스크립트 guid 역참조 0건 |

> ⚠️ 즉 `BossEncounterController.Start()` 자동 시작(D8)은 **아직 발현되지 않은 잠복 문제**였다. 보스 전투는 현재 씬에 연결조차 되어 있지 않다.

**적용한 설계 — 씬 수정 없이 진입점 통일**
- `FlowStep`: `Awake()`는 큐 수집만, 자동 시작은 `Start()`로 이동하고 `_autoStartOnSceneLoad` 플래그로 감쌌다. 필드 기본값이 `true`라 **기존 씬 인스턴스는 그대로 동작**한다(한 프레임 늦어질 뿐).
- `FlowManager`가 인스턴스화할 때는 `PrepareExternalStart(level)`로 자동 시작을 끄고 레벨을 주입한 뒤 명시적으로 시작 → **D6 이중 실행, D7 `_flowLevel` 항상 0 동시 해소**.
- `GameBootstrapper`가 `Awake`에서 씬의 `FlowStep`들에 `CancelAutoStart()`를 걸 수 있어, 부팅 프로필이 씬 기본 동작을 **덮어쓸 수 있다**(`Awake`는 모든 `Start`보다 먼저 돈다는 순서 보장 이용).

**구현 내용**
| 파일 | 변경 |
|---|---|
| `FlowStep.cs` | 자동 시작 분리, `CancelAutoStart()`, `PrepareExternalStart()`, `IsRunning`, 중복 시작 차단 |
| `FlowManager.cs` | `StartChapter(chapterId, step)`, 범위 검증(TODO 해소), 스텝 자동 진행 체인, `StopFlow()`, `CaptureSnapshot()`/`RestoreSnapshot()`, 시작/종료/챕터종료 이벤트 |
| `BossEncounterController.cs` | `Start()` 자동 시작 제거 → `StartEncounter(skipIntro, startPhaseIndex)` 공개, `JumpToPhase()`(툴용), 인트로 스킵 |
| `CombatManager.cs` | `StartCombat()` 실장(스텁 해소), `SetCombatScene()`/`SetDebugOptions()` |

#### T1 — `BootProfile` ✅ 코드 완료 (씬 연결 대기)
게임이 **어떤 상태로 시작할지**를 데이터 하나로 표현한다. 런타임은 이것만 보고 부팅하고, 에디터 창은 이 값을 채우는 UI일 뿐이다.

```
BootMode { Normal, CombatOnly, FlowFrom }

BootProfile (SO/Boot/BootProfile)
  mode
  combatScene, startPhaseIndex   // CombatOnly
  chapterId, step                // FlowFrom
  skipBossIntro
```

- **순수 전투만** = `CombatOnly` — Flow를 건너뛰고 `CombatManager.StartCombat()`으로 직행, 보스 인트로 대사·시네마틱도 선택적으로 스킵.
- **특정 시점부터** = `FlowFrom` — `FlowManager.StartChapter(chapterId, step)`. 기존 `FlowSnapshot`을 그대로 쓰는 `RestoreSnapshot()`도 함께 열어뒀다.
- `Normal`은 **아무것도 하지 않는다** — 씬이 지금까지 하던 대로 굴러간다(회귀 위험 0).

**설계 의도** — 에디터 전용 코드가 런타임 로직에 섞이지 않게 했다(`#if UNITY_EDITOR` 오염 방지). 같은 프로필을 치트 콘솔·자동화 테스트가 재사용할 수 있다.

**씬 연결 ✅ 2026-09-18** — `Tools/Unorder/Test/Setup Temp Boss (2 Patterns)` 메뉴로 일괄 처리(재실행 안전).
- `GameBootstrapper` ← `BootProfile_CombatOnly`(skipBossIntro) · `CombatManager` · `FlowManager` 바인딩
- `BossEncounter`(신규) · `CombatManager`(ManagerInitializer 하위) 배치
- **플레이 검증**: 에러 0 · Flow 미실행(CombatOnly 정상) · 보스 탄 발사 확인(Aimed 속도 9로 플레이어 방향 이동)
- ⚠️ 씬은 **저장하지 않음**. 원래 흐름으로 되돌리려면 `GameBootstrapper._profile`을 비우거나 프로필 `mode`를 `Normal`로.

#### T1.5 — 보스 구동을 BT에서 패턴 러너로 교체 ✅ 2026-09-18
- 근거·구조는 [architecture §4](architecture/README.md). BT 노드 5종·`Boss_1_Agent` 그래프 삭제, `Boss_UNMOVE`의 `BehaviorGraphAgent` 제거.
- 임시 보스: `Boss_UNMOVE` + 패턴 2종(`TempPattern_Aimed` 5연발 / `TempPattern_Ring` 12발×3링) · 순차 선택 · 페이즈 1개 (`Assets/08.SO/Boss/Temp/`)
- 탄: `Projectile_Boss1_Spear`(드래그 선택 가능한 `SelectableProjectile`). `Projectile_Boss1`은 풀 미등록이라 제외.
- **사용자 몫**: `com.unity.behavior` 패키지 제거.

#### T2 — 테스트 에디터 창 (`Util/Unorder TestKit`) 📋 미착수
> **다음 작업: 패턴 관리 에디터.** BT 폐기로 사라진 "흐름을 눈으로 보는" 기능을 대체한다. 페이즈별 패턴 목록·가중치·딜레이 편집, 선택 전략 교체, 플레이 중 `BossPatternRunner.RequestPattern`/`SetPaused`로 특정 패턴만 반복 실행. 아래 Combat 탭과 겸한다.
기존 `UtilityWindow`의 **`partial class` + 툴바 enum 확장 패턴**을 따른다(전례: `SoundSystem/Editor/UtilityWindow.cs`). 다만 그 창은 `MINISoundManage` 네임스페이스·사운드 전용이므로 **별도 창**으로 만들고 패턴만 차용한다.

| 탭 | 기능 | 선행 |
|---|---|---|
| **Boot** | 부팅 모드 선택 → 플레이. CombatOnly(전투 SO 선택) / FlowFrom(챕터·스텝 드롭다운) | T1 씬 연결 |
| **Combat** | 보스 페이즈 강제 전환(`JumpToPhase` 준비됨), **패턴 단독 반복 실행**, 무적 토글, Point 즉시 지급, 탄막 일괄 제거, 턴 강제 전환 | M1 |
| **Rule** | 룰 on/off 토글 · 현재 활성 룰 목록 · 발동 로그 | M2 |
| **Flow** | 스텝 점프, 현재 큐 스킵, 스냅샷 저장/로드(`CaptureSnapshot` 준비됨) | T0 ✅ |

> **Rule 탭이 실질적으로 가장 중요하다.** 애드혹 룰이 계속 늘어나는 구조([design/core-design.md §3.1](design/core-design.md))에서, 룰 조합을 즉석에서 켜고 끌 수 없으면 조합 폭발을 검증할 방법이 없다.

**DoD** — 에디터에서 **클릭 2회 이내로** ① 임의의 보스 전투만 단독 실행, ② 임의의 챕터·스텝부터 Flow 재생, ③ 임의의 룰 조합을 켠 상태로 시작할 수 있다.

---

## 3. 크리티컬 패스

```
M0(기획 잠금)
  └→ T0 ✅ → T1 ✅(씬 연결 대기) → T2 Boot탭
        └→ M1(턴 루프) ──┐        ↑ T2 Combat탭
                         ├→ M2(Rule 골격) → M3(수직 슬라이스) → M4(보스 완성) → M5(런 구조) → M6(폴리시)
        └────────────────┘        ↑ T2 Rule탭
```
- **M0 → M2**가 진짜 병목. M2의 훅 설계 품질이 M4 이후 전체 비용을 결정한다.
- T0에서 `CombatManager.StartCombat()`이 실장되어 **M1의 선행 항목 하나가 미리 해소**됐다.
- M6의 일부(GC/프로파일링)는 어느 단계에서든 병행 가능.

---

## 4. 병렬 기술부채 트랙

### ✅ 해소됨 (2026-09-18)
| # | 항목 | 조치 |
|---|---|---|
| D1 | 체력 0에서 죽지 않음 | `HealthBody.cs` 사망 판정 `< 0` → `<= 0` |
| D2 | `ActionType` 이름 중복(2개 네임스페이스) | `FlowSystem` 쪽을 `FlowActionType`으로 개명 (사용처 0곳 확인 후) |
| D3 | `CombatScene.condfig` 오타 | `config`로 수정 (참조 `.asset` 0개 확인 후 — 직렬화 손실 없음) |
| D4 | `ActionType.Destory` 오타 | `Destroy`로 수정 |
| D6 | 첫 큐 이중 실행(`Awake`+`FlowManager` 양쪽 시작) | T0 — 자동 시작을 `Start()`로 분리하고 외부 시작 시 차단 |
| D7 | `SetFlowLevel` 미호출로 `_flowLevel` 항상 0 | T0 — `PrepareExternalStart(level)`로 주입 |
| D8 | `BossEncounterController` 무조건 자동 시작 | T0 — `Start()` 제거, `StartEncounter()` 공개 |
| — | `FlowManager`의 `// TODO : Check Range` | T0 — 챕터/스텝 범위 검증 실장 |
| D11 | `Boss.SetVariable()`이 변수 없을 때 `Debug.Assert` 후 null 접근 → NRE | T1.5 — BT 폐기로 코드 자체 제거 |

### 미해소
| # | 항목 | 위치 | 비고 |
|---|---|---|---|
| D5 | `docs/data/` 데이터 스펙 내용 | [data/README.md](data/README.md) | 인덱스만 생성됨, 필드 의미 서술 필요 |
| D9 | `ConditionAttribute`의 enum 비교가 `(int)ToCompare` 언박싱이라 enum을 직접 넘기면 런타임 예외 | `Core/CustomAttribute/Editor/ConditionDrawer.cs:46` | `BootProfile`은 `(int)` 캐스팅으로 우회 중. 드로어 수정이 정석 |
| D10 | `FlowStep`이 `Invoke(nameof(...))` 문자열 호출 + `Time.timeScale` 영향 | `FlowStep.cs` | 일시정지/배속과 충돌 가능 |

---

## 5. 갱신 규칙
- 마일스톤 착수 시 제목에 🚧, 완료 시 ✅를 붙이고 완료일을 적는다.
- DoD를 바꿀 때는 **왜 바뀌었는지**를 함께 남긴다(한 줄 요약 금지 — ACTIVE_GUARD §3).
- M0에서 기획이 확정되면 [core-design.md §8](design/core-design.md)의 해당 항목을 "해소됨"으로 옮긴다.
