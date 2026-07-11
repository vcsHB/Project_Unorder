# Conventions — 코딩 규약 · 반복 실수 로그

> 트리거: **같은 실수를 2번 이상** 반복하면 이 문서에 규칙으로 승격한다 (원인 + 적용법 기록).

## 1. 네이밍
| 대상 | 규칙 | 예 |
|---|---|---|
| 클래스 / 메서드 | PascalCase | `PlayerController`, `UpdateInventory` |
| 지역 변수 / 파라미터 | camelCase | `isDead`, `moveSpeed` |
| private 필드 | `_camelCase` | `_health`, `_rigidbody` |
| 상수 / Enum | PascalCase | `MaxHp`, `PlayerStateType` |

## 2. 네임스페이스
- 루트: `Project_Unorder.<System>[.<SubSystem>]`
  - 예: `Project_Unorder.AgentSystem.BossSystem.AttackPattern`, `Project_Unorder.UIManage.InGameSceneUI.MainUIs`
- 예외(레거시/외부 유틸): `MINISoundManage`, `ObjectPooling`, `DataManage`, `Core.TextUtil`, `HAM_DeBugger.KetchupHierachy` — 신규 코드는 예외를 늘리지 말 것.

## 3. 성능
- **GC 최소화**: `Update`/`FixedUpdate` 루프 내 `new`, LINQ, GC 유발 `foreach` 지양. `for` 우선. 가독성이 크게 필요한 곳만 예외.
- **메모리 정렬**: struct 사용 시 패딩을 고려해 필드 배치.
- 반복 생성 오브젝트는 `ObjectPooling`(`PoolManager`/`IPoolable`)을 통해 재사용.

## 4. Unity 패턴
- 필수 컴포넌트 의존은 `[RequireComponent(typeof(...))]`로 명시.
- 싱글턴은 `MonoSingleton<T>` 사용. 순서 의존 초기화는 `IEarlyAwakeableManager.PreAwake()`.
- 인게임 엔티티 로직은 `Agent` + `IAgentComponent` 컴포넌트 패턴으로 조립. 컴포넌트 조회는 `GetCompo<T>()`.
- 데이터/설정값은 ScriptableObject(`*SO`, `*Data`)로 분리.

## 5. 작성 원칙
- **주석 없이** 논리 구조만으로 의도가 드러나게 작성. 설명이 필요하면 이름/구조를 먼저 고친다.
- 기존 코드의 주석 밀도·네이밍·관용구를 따라간다.

## 6. 코딩 전 사고
- **추측하지 말 것.** 모르는 건 코드/파일로 확인한다.
- **혼란을 숨기지 말 것.** 애매하면 애매하다고 말하고, 트레이드오프를 드러낸다.
- 구현 전에 **가정을 명확히** 한다. 불확실한 부분은 진행하지 말고 사용자에게 질문한다.

## 7. 변경 범위
- **요청한 코드만** 수정한다. 요청하지 않은 부수 작업(리팩토링·개선·제거)을 끼워 넣지 않는다.
- 추가 수정이 필요하다고 판단되면 **의견으로 제시**한다. 제시 자체는 권장.
- 사용자가 **명시적으로 거부하지 않는 한**, 해소되지 않은 의견은 계속 어필한다 (한 번 말하고 흘리지 않음).

## 8. 변경 전 설명 (승인 후 진행)
아래 중 하나라도 해당하면, 먼저 계획을 설명하고 **승인을 받은 뒤** 진행한다:
- 3개 이상 파일 동시 수정
- 아키텍처 · 매니저 · 공용 모듈 변경
- 파일 / 함수 / 필드 삭제, 함수 시그니처 변경
- 기존 동작 변경 (단순 추가가 아닌 경우)

---

## 반복 실수 로그 (2회 이상 → 규칙 승격)

> 형식: `실수` — **원인** — **적용법**. 1회는 여기 관찰만, 2회째 위 규칙 섹션으로 승격.

_아직 기록 없음._
