# Data — 데이터 스펙

> ScriptableObject / 직렬화 데이터의 **스키마와 의미**를 기록한다.
> 갱신 트리거: **데이터 스키마 변경**. 코드 구조는 [../architecture/](../architecture/README.md), 기획 의도는 [../design/](../design/README.md).
> ⚠️ `.asset` 파일 자체는 텍스트 편집 금지 (ACTIVE_GUARD G2) — Unity/MCP로 다룬다.

## 현재 SO 자산 (`Assets/08.SO/`)

| 경로 | 타입 | 용도 |
|---|---|---|
| `Boss/BossData.asset` | `BossDataSO` | 보스 기본 데이터 |
| `Boss_1_Agent.asset` | — | 보스 1 에이전트 데이터 |
| `Flow/Chapter_0.asset` | `ChapterData` | 챕터 0의 `FlowStep` 배열 |
| `Flow/_ChapterGroup.asset` | `ChapterGroupData` | 챕터 묶음 |
| `Informations/InformationDataGroup.asset` | `DataGroupSO` | 정보 데이터 그룹 |
| `ObjectPool/CombatScenePool.asset` | `PoolGroupSO` | 전투 씬 풀 구성 |
| `Player/PlayerData_Order.asset` · `_Unorder.asset` | `PlayerDataSO` | 플레이어별 스탯 |
| `Projectile/Projectile_*.asset` | `ProjectileSO` | 탄막 풀·스펙 |
| `Sound/*` | `SoundTableSO` 외 | BGM/SFX 테이블 |

## 미작성 스펙 (TODO)
- `BossDataSO` / `BossEncounterDataSO` / `BossPhaseDataSO` 필드 의미와 작성 규칙
- `BossAttackPatternSO` 파생 4종(Aimed·Ring·Spread·Wave)의 파라미터 의미
- `ProjectileSO` 풀 크기 산정 기준
- `CombatScene` SO — 전투 단위 정의 (T트랙 `BootProfile`의 입력, [../milestones.md](../milestones.md) 참조)
- Point / 턴 관련 데이터 (M0 기획 확정 후)

> 새 SO 타입을 추가하면 위 표에 등록하고, 필드 의미를 이 문서에 적는다.
