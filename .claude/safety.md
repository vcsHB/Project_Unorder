# Safety — 위험 명령 · 사고 패턴

> 트리거: **위험 명령 / 사고 패턴 발견** 시 이 문서를 갱신한다 (재발 방지).

## 절대 금지 / 확인 필수

| 위험 | 이유 | 대안 |
|---|---|---|
| `.meta` 파일 삭제·생성·이동 | Unity GUID 참조 붕괴 → 자산 링크 전부 깨짐 | 자산은 Unity 에디터/MCP로 이동·삭제 |
| 씬/프리팹/SO 텍스트 편집 | 직렬화 손상 | MCP(`update_component` 등) 사용 |
| `git reset --hard`, `git clean -fdx`, 강제 푸시 | 작업물/미커밋 자산 소실 | 먼저 상태 확인, 사용자 승인 |
| 기본 브랜치(main)에 직접 커밋 | 히스토리 오염 | 브랜치 분기 후 작업 |
| `Library/`, `Temp/`, `obj/` 삭제 | 대용량 재생성/재임포트(시간 소모) | 필요 시에만, 사용자 고지 |
| 대량 파일 일괄 치환(sed/정규식) | 의도치 않은 코드 변형 | 범위 좁혀 Edit, 결과 확인 |
| MCP `recompile_scripts` 연속 호출 | 도메인 리로드가 MCP 소켓을 고아로 만들어 **MCP 자체가 끊김** (아래 사고 로그 #1) | 저장 후 Unity가 포커스에서 자동 컴파일 → `get_console_logs`로 결과 확인 |

## 환경 주의
- 플랫폼 Windows / 셸 PowerShell 우선. POSIX는 Bash 도구로 분리.
- Unity MCP는 **에디터 실행 중**에만 동작. 미실행 시 실패는 사고가 아님.
- MCP가 죽었을 때 컴파일 검증 대안: `dotnet build Assembly-CSharp.csproj` (출력은 스크래치패드로 리다이렉트). Unity 참조 어셈블리를 그대로 쓰므로 신뢰 가능. 단 `Assembly-CSharp-Editor`는 별도이므로, Editor 스크립트가 변경 타입을 참조하는지 먼저 확인할 것.
- 커밋 메시지·PR은 사용자가 요청할 때만.

## 사고 로그

> 형식: `무슨 일` — **원인** — **재발 방지책**.

### #1 (2026-09-18) Unity MCP 연결 단절 — 에디터 재시작으로 복구 안 됨
- **무슨 일**: `recompile_scripts` 1회 성공 후 모든 MCP 호출이 `Connection closed`. **Unity를 재시작해도 그대로**였다.
- **측정된 증상**
  - `netstat` — 포트 8090이 **이미 죽은 PID**(구 Unity.exe)로 `LISTENING` 유지
  - `127.0.0.1:8090` → `ECONNREFUSED` (IPv4 미바인딩. `AllowRemoteConnections:false`면 정상)
  - `[::1]:8090` → **TCP는 붙지만 WebSocket 업그레이드 핸드셰이크 타임아웃**
- **진짜 원인 — 소켓 핸들 상속**: Unity가 스크립트를 열려고 **VS Code를 자식 프로세스로 실행**할 때, Unity의 리스닝 소켓 핸들이 자식에게 **상속**된다. 이후:
  1. 도메인 리로드(= `recompile_scripts`)로 Unity가 `StopServer()` → 하지만 **VS Code가 쥔 핸들 때문에 포트는 계속 바인딩 상태**
  2. 리로드 후 `StartServer()`는 `AddressAlreadyInUse`로 실패 (`McpUnityServer.cs`에서 로그만 남기고 조용히 반환)
  3. 결과: 포트는 열려 있으나 **핸들러가 없는 고아 리스너** → TCP accept는 되고 핸드셰이크는 영영 안 끝남
  4. Unity를 재시작해도 **핸들 보유자(VS Code)가 살아 있는 한 8090은 계속 점유**된다
- **범인 특정 방법** (재발 시 이 순서로):
  ```
  netstat -ano | grep :8090                  # 소유 PID 확인
  tasklist /FI "PID eq <그 PID>"             # 이미 죽은 PID면 상속 의심
  Get-CimInstance Win32_Process | Where-Object { $_.ParentProcessId -eq <죽은 PID> }
  ```
  마지막 줄이 **살아남은 자식 프로세스 = 핸들 보유자**를 알려준다.
- **복구책** (효과 순)
  1. **핸들 보유자(보통 VS Code) 재시작** — 소켓이 풀린다. 그 후 Unity에서 Start Server. ✅ 2026-09-18 이 방법으로 복구 확인
  2. **포트 변경** — Server Window에서 Port를 바꿔 고아 소켓을 우회. 세션을 유지해야 할 때.
  3. ~~Unity 에디터 재시작~~ — **효과 없음.** 점유자는 Unity가 아니다.
- **재발 방지책**
  1. MCP로 `recompile_scripts`를 습관적으로 부르지 말 것. 저장 후 Unity 포커스로 자동 컴파일되며 결과는 `get_console_logs`로 읽는다. (도메인 리로드가 위 연쇄의 방아쇠다)
  2. MCP가 죽었을 때 컴파일 검증은 `dotnet build Assembly-CSharp.csproj`로 대체 가능.
  3. **VS Code를 Unity에서 띄우지 말 것.** 스크립트 더블클릭·Open C# Project로 VS Code가 *처음* 실행되면 Unity의 자식이 되어 소켓을 상속한다. VS Code는 시작 메뉴/작업 표시줄에서 먼저 연다. 이미 떠 있는 VS Code로 스크립트를 넘기는 건 전달용 프로세스가 곧 종료되므로 안전하다.
  4. 점검 명령: `Get-CimInstance Win32_Process | Where-Object { $_.ParentProcessId -eq <Unity PID> -and $_.Name -eq "Code.exe" }` — 결과가 있으면 스크립트 저장 한 번에 MCP가 끊긴다.
- **재발 기록**: 같은 날 01:49 VS Code(PID 54580)가 다시 Unity 자식으로 실행된 것을 확인. Claude 세션과 무관하게 **VS Code 실행 경로**가 원인임이 재확인됨.

### #2 (2026-09-18) 플레이 중 재컴파일로 가짜 NullReferenceException
- **무슨 일**: 코드 추가 직후 MCP로 Play 진입 → `PlayerStateMachine.UpdateState()`(`PlayerStateMachine.cs:54`)에서 매 프레임 NRE. 플레이어 코드는 바뀐 적이 없었다.
- **원인**: Editor.log 확인 결과, 씬 `Awake`가 정상 실행된 **뒤에** `Assetdatabase observed changes in script compilation related files`로 **플레이 중 재컴파일 + 도메인 리로드**가 일어났다. 리로드는 직렬화 안 된 상태(`CurrentState` 프로퍼티)를 날리고, `[SerializeField]`인 `_stateMachine` 껍데기만 남아 null 접근이 났다. 스크립트 추가·삭제 직후 Unity가 변경 감지를 늦게 끝내 플레이와 겹친 것.
- **판별법**: Editor.log에서 `[ManagerInitializer] PreAwake Run` **다음에** `Reloading assemblies after forced synchronous recompile`이 있으면 이 사고다. 코드 버그로 오진하지 말 것.
- **재발 방지책**
  1. 스크립트 변경 후 플레이 테스트 전에 `get_console_logs`로 컴파일 완료를 확인하고, 한 번 정지 → 재진입해 **깨끗한 세션**에서 판단한다.
  2. Unity `Preferences > General > Script Changes While Playing`을 `Recompile After Finished Playing`으로 두면 원천 차단된다 (사용자 설정 — 권장).
  3. 플레이 중 에러를 조사할 때 MCP 콘솔은 도메인 리로드 때마다 초기화되므로, **전체 순서는 `%LOCALAPPDATA%\Unity\Editor\Editor.log`로 본다.**
