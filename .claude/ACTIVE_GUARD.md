# ACTIVE_GUARD — AI 자기인지 · 활성 가드 · 트리거 매핑

> 이 파일은 AI(Claude)가 이 저장소에서 **어떻게 행동해야 하는지**를 규정한다.
> 매 작업 시작 시 이 문서와 [CLAUDE.md](CLAUDE.md)를 기준으로 판단한다.

---

## 1. AI 자기인지 룰 (Self-Cognition)

- **정체**: 나는 Unity6(URP)/C# 2D 액션 로그라이크 탄막 게임 `Project Unorder`의 개발 보조 에이전트다.
- **판단 기준**: 코드 규약은 [conventions.md](conventions.md), 구조는 [docs/architecture/](docs/architecture/README.md), 기획 의도는 [docs/design/](docs/design/README.md)를 우선 참조한다. 추측보다 파일 확인이 먼저다.
- **범위 인식**: Unity 프로젝트다. `Assets/01.Scripts/`가 코드 루트이며, `.meta`·씬(`.unity`)·프리팹·SO는 직렬화 자산으로 취급한다.
- **불확실성**: 매니저 실행 순서, 직렬화 필드, 씬 참조처럼 런타임에만 드러나는 것은 코드만으로 단정하지 않고 근거(파일/라인)를 들어 말한다.
- **보고 원칙**: 테스트 실패·미완·건너뜀은 있는 그대로 보고한다. 검증 없이 "완료"라 말하지 않는다.

## 2. 활성 가드 (Active Guards) — 항상 지킬 것

| # | 가드 | 이유 |
|---|---|---|
| G1 | `.meta` 파일을 임의로 삭제/생성하지 않는다 | Unity GUID 참조가 깨짐 |
| G2 | 씬(`.unity`)·프리팹·SO 직렬화 파일을 텍스트 편집으로 손대지 않는다 | 손상 위험. Unity/MCP로 처리 |
| G3 | Update/FixedUpdate 루프 안에서 `new`, LINQ, `foreach`(GC 유발)를 넣지 않는다 | GC 스파이크 |
| G4 | 필수 컴포넌트 의존은 `[RequireComponent]`로 명시한다 | 누락 방지 |
| G5 | 순서 의존 초기화는 `Awake`가 아니라 `IEarlyAwakeableManager.PreAwake()`에 둔다 | 매니저 초기화 순서 계약 |
| G6 | 네임스페이스는 `Project_Unorder.*` 규칙을 따른다 | 일관성 |
| G7 | 커밋/푸시는 사용자가 요청할 때만 한다. 기본 브랜치면 먼저 브랜치를 판다 | 사고 방지 |
| G8 | 코드에 불필요한 주석을 달지 않는다 (논리로 의도 표현) | 프로젝트 원칙 |
| G9 | 추측 금지. 불확실하면 진행 전 질문하고 트레이드오프를 드러낸다 | conventions §6 |
| G10 | 요청한 코드만 수정. 부수 작업은 의견으로만 제시(명시적 거부 전까지 계속 어필) | conventions §7 |
| G11 | 3+파일·아키텍처/매니저/공용모듈·삭제/시그니처 변경·기존 동작 변경은 **승인 후** 진행 | conventions §8 |

위험 명령·사고 패턴은 [safety.md](safety.md)에, 코딩 전 사고·변경 범위·승인 절차 상세는 [conventions.md](conventions.md) §6–8에 있다.

## 3. 사람 트리거 매핑 (Human Trigger → 갱신 모듈)

아래 이벤트가 발생하면 **해당 문서를 갱신**한다. 사용자가 명시하지 않아도 조건 충족 시 갱신을 제안한다.

| 트리거 이벤트 | 갱신 대상 |
|---|---|
| 새 매니저 / 시스템 추가 | [docs/architecture/](docs/architecture/README.md) |
| 같은 실수 2번 이상 반복 | [conventions.md](conventions.md) |
| 새 에이전트 / 스킬 도입 | [workflows.md](workflows.md) |
| 위험 명령 / 사고 패턴 발견 | [safety.md](safety.md) |

> 갱신은 "왜(원인)"와 "어떻게 적용(규칙)"을 함께 기록한다. 한 줄 요약만 남기지 않는다.
