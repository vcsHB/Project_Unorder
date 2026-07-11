# Workflows — 에이전트 · 스킬 · 도구

> 트리거: **새 에이전트/스킬 도입** 시 이 문서를 갱신한다.

## 1. Unity MCP (`mcp-unity`)
- 설정: [.mcp.json](../.mcp.json) — `com.gamelovers.mcp-unity` 패키지의 Node 서버. Unity 에디터가 켜져 있어야 동작한다.
- 용도: 씬/게임오브젝트/컴포넌트/머티리얼 조작, 콘솔 로그 조회, 테스트 실행, 스크립트 리컴파일.
- **원칙**: 씬(`.unity`)·프리팹·머티리얼은 텍스트 편집 대신 MCP 도구로 다룬다 (ACTIVE_GUARD G2).
- 자주 쓰는 도구:
  - `get_scene_info` / `get_gameobject` / `select_gameobject` — 현재 상태 파악
  - `update_component` / `update_gameobject` — 값 수정
  - `get_console_logs` — 컴파일/런타임 에러 확인
  - `run_tests` — 테스트 실행
  - `recompile_scripts` — 스크립트 변경 후 반영

## 2. 서브 에이전트 (Agent 도구)
- `Explore` — 넓은 범위 코드/파일 탐색(결론만 필요할 때). 읽기 전용.
- `Plan` — 구현 전략/설계.
- `general-purpose` — 다단계 탐색·작업.
- `claude-code-guide` — Claude Code/SDK/API 사용법 질문.
- 규칙: 사용자가 서브에이전트/워크플로우를 **명시적으로 요청**할 때만 스폰한다. 일반 작업은 인라인 도구로 처리.

## 3. 스킬 (Skill 도구)
- `/code-review` — 현재 diff 리뷰(정확성 + 정리). `/code-review ultra`는 클라우드 멀티에이전트 리뷰(사용자 트리거·과금).
- `/simplify` — 변경 코드 품질 정리.
- `/verify`, `/run` — 변경을 실제 앱에서 구동/검증.
- `/init` — CLAUDE.md 초기화.
- 사용자가 `/<skill-name>`을 입력하면 해당 스킬을 호출한다. 목록에 없는 스킬은 추측하지 않는다.

## 4. 커스텀 워크플로우
- [.claude/workflows.md](workflows.md) (이 파일) 및 프로젝트 워크플로우 스크립트가 있으면 여기 링크로 정리한다.

_신규 에이전트/스킬 추가 시 이 아래에 항목을 덧붙인다._
