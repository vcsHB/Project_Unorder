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
| 보스 전투 구동 | `Boss.cs`(BehaviorGraph) + `BossEncounterController`(페이즈/인트로/대사/시네마틱) |
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

#### T0 — 부팅 진입점 단일화 🚧 선행 과제
툴을 만들기 전에 **시작 지점을 하나로 모으는 것**이 먼저다. 지금은 진입점이 흩어져 있어 "어디서부터 시작"을 제어할 수 없다:

| 현재 문제 | 근거 |
|---|---|
| `FlowStep`이 씬에 놓이기만 하면 `Awake()`에서 **스스로 시작** | `FlowStep.cs:24` |
| `FlowManager`를 **호출하는 코드가 0곳** — Flow는 오케스트레이션 없이 씬 배치로만 굴러감 | `SetCurrentChapter`/`StartFlow` 외부 호출부 없음 |
| 보스가 씬 진입 즉시 인트로부터 자동 시작 | `BossEncounterController.Start()` → `StartEncounter()` |

**할 일** — 자동 시작을 걷어내고 `FlowManager`/`CombatManager`가 진입을 **주도**하게 한다. (D6·D8 해소와 동일 작업)

#### T1 — `BootProfile` (런타임 개념, 에디터 아님)
게임이 **어떤 상태로 시작할지**를 데이터 하나로 표현한다. 런타임은 이것만 보고 부팅하고, 에디터 창은 이 값을 채우는 UI일 뿐이다.

```
BootMode { Normal, CombatOnly, FlowFrom }

BootProfile
  mode
  combatScene      : CombatScene SO   // CombatOnly
  chapterId, step  : uint             // FlowFrom
  skipIntro / skipDialogue / skipCinematic : bool
  forcedRules      : (M2 이후) 시작 시 활성화할 룰 목록
```

**설계 의도** — 에디터 전용 코드가 런타임 로직에 섞이지 않게 한다(`#if UNITY_EDITOR` 오염 방지). 같은 프로필을 나중에 치트 콘솔·자동화 테스트가 재사용할 수 있다.

- **순수 전투 진행** = `mode: CombatOnly` — Flow·대사·시네마틱을 건너뛰고 `CombatManager.StartCombat()`으로 직행.
- **특정 시점부터 Flow 진행** = `mode: FlowFrom` — 이미 존재하는 `FlowSnapshot(flowChapter, step, mapId, layerIndex, position)`을 **주입**하는 형태. 새 개념을 만들 필요 없이 기존 자료구조를 쓴다.

#### T2 — 테스트 에디터 창 (`Util/Unorder TestKit`)
기존 `UtilityWindow`의 **`partial class` + 툴바 enum 확장 패턴**을 따른다(전례: `SoundSystem/Editor/UtilityWindow.cs`). 다만 그 창은 `MINISoundManage` 네임스페이스·사운드 전용이므로 **별도 창**으로 만들고 패턴만 차용한다.

| 탭 | 기능 | 선행 |
|---|---|---|
| **Boot** | 부팅 모드 선택 → 플레이. CombatOnly(전투 SO 선택) / FlowFrom(챕터·스텝 드롭다운) | T0·T1 |
| **Combat** | 보스 페이즈 강제 전환, **패턴 단독 반복 실행**, 무적 토글, Point 즉시 지급, 탄막 일괄 제거, 턴 강제 전환 | M1 |
| **Rule** | 룰 on/off 토글 · 현재 활성 룰 목록 · 발동 로그 | M2 |
| **Flow** | 스텝 점프, 현재 큐 스킵, 스냅샷 저장/로드 | T0 |

> **Rule 탭이 실질적으로 가장 중요하다.** 애드혹 룰이 계속 늘어나는 구조([design/core-design.md §3.1](design/core-design.md))에서, 룰 조합을 즉석에서 켜고 끌 수 없으면 조합 폭발을 검증할 방법이 없다.

**DoD** — 에디터에서 **클릭 2회 이내로** ① 임의의 보스 전투만 단독 실행, ② 임의의 챕터·스텝부터 Flow 재생, ③ 임의의 룰 조합을 켠 상태로 시작할 수 있다.

---

## 3. 크리티컬 패스

```
M0(기획 잠금)
  └→ T0(진입점 단일화) → T1(BootProfile) → T2 Boot탭
        └→ M1(턴 루프) ──┐        ↑ T2 Combat탭
                         ├→ M2(Rule 골격) → M3(수직 슬라이스) → M4(보스 완성) → M5(런 구조) → M6(폴리시)
        └────────────────┘        ↑ T2 Rule탭
```
- **M0 → M2**가 진짜 병목. M2의 훅 설계 품질이 M4 이후 전체 비용을 결정한다.
- **T0는 M1보다 먼저** 하는 게 싸다. `CombatManager.StartCombat()` 실장이 M1 항목이자 T0 항목이라 어차피 같은 파일을 연다.
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

### 미해소
| # | 항목 | 위치 | 비고 |
|---|---|---|---|
| D5 | `docs/data/` 데이터 스펙 문서 미작성 | `.claude/docs/data/` | 인덱스만 생성됨, 내용 필요 |
| D6 | **첫 큐 이중 실행** — `FlowStep.Awake()`가 `StartFlow()`를 호출하는데 `FlowManager.StartFlow()`도 호출 → 구독·`Execute()` 2회 | `FlowStep.cs:24`, `FlowManager.cs:25` | **T0에서 해소** |
| D7 | `FlowStep.SetFlowLevel()`을 아무도 호출하지 않아 `_flowLevel`이 항상 0 (로그·분기 오염) | `FlowStep.cs`, `FlowManager.cs` | **T0에서 해소** |
| D8 | `BossEncounterController.Start()`가 무조건 인트로 시작 → 전투 단독 실행 불가 | `BossEncounterController.cs:22` | **T0에서 해소** |

> D6·D7·D8은 단순 버그가 아니라 **T트랙의 전제**다. 개별 수정 대신 T0에서 한꺼번에 처리한다.

---

## 5. 갱신 규칙
- 마일스톤 착수 시 제목에 🚧, 완료 시 ✅를 붙이고 완료일을 적는다.
- DoD를 바꿀 때는 **왜 바뀌었는지**를 함께 남긴다(한 줄 요약 금지 — ACTIVE_GUARD §3).
- M0에서 기획이 확정되면 [core-design.md §8](design/core-design.md)의 해당 항목을 "해소됨"으로 옮긴다.
