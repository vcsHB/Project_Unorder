using System.Text;
using Project_Unorder.AgentSystem.BossSystem.AttackPattern;
using Project_Unorder.AgentSystem.BossSystem.AttackPattern.Patterns;
using Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection;
using Project_Unorder.AgentSystem.BossSystem.Data;
using Project_Unorder.AgentSystem.BossSystem.Shooter;
using Project_Unorder.AgentSystem.PlayerManage;
using Project_Unorder.BootSystem;
using Project_Unorder.CombatSystem.Core;
using Project_Unorder.CombatSystem.ProjectileSystem;
using Project_Unorder.FlowSystem;
using Project_Unorder.LogSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project_Unorder.AgentSystem.BossSystem.EditorScripts
{
    public static class TempBossSetup
    {
        private const string TempFolder = "Assets/08.SO/Boss/Temp";
        private const string TargetSceneName = "CombatScene";
        private const string BossObjectName = "Boss_UNMOVE";
        private const string EmitterObjectName = "Emitter";
        private const string EncounterObjectName = "BossEncounter";
        private const string CombatManagerObjectName = "CombatManager";
        private const string LegacyGraphPath = "Assets/08.SO/Boss_1_Agent.asset";
        private const string LegacyAgentTypeName = "Unity.Behavior.BehaviorGraphAgent";
        private const string OrderPlayerDataPath = "Assets/08.SO/Player/PlayerData_Order.asset";
        private const string BossProjectilePath = "Assets/08.SO/Projectile/Projectile_Boss1_Spear.asset";
        private const string BootProfilePath = "Assets/08.SO/Game/BootProfile_CombatOnly.asset";

        [MenuItem("Tools/Unorder/Test/Setup Temp Boss (2 Patterns)")]
        public static void Setup()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name != TargetSceneName)
            {
                Debug.LogError($"[TempBossSetup] Open {TargetSceneName} first. Current: {scene.name}");
                return;
            }

            StringBuilder report = new StringBuilder("[TempBossSetup] Done\n");

            EnsureFolder(TempFolder);
            AimedPatternSO aimed = CreateAimedPattern();
            RingPatternSO ring = CreateRingPattern();
            SequentialPatternSelectorSO selector = LoadOrCreate<SequentialPatternSelectorSO>($"{TempFolder}/TempSelector_Sequential.asset");
            BossPhaseDataSO phase = CreatePhase(aimed, ring, selector);
            BossEncounterDataSO encounterData = CreateEncounterData(phase);
            CombatScene combatScene = CreateCombatScene();
            report.AppendLine($"- Assets: {TempFolder}");

            Boss boss = SetupBoss(scene, report);
            if (boss == null) return;

            BossEncounterController encounter = SetupEncounter(scene, boss, encounterData);
            CombatManager combatManager = SetupCombatManager(encounter);
            report.AppendLine("- Scene: BossEncounter, CombatManager bound");

            BindBootProfile(combatScene, combatManager, report);
            DeleteLegacyGraph(report);

            AssetDatabase.SaveAssets();
            EditorSceneManager.MarkSceneDirty(scene);
            report.AppendLine("- Scene marked dirty (not saved)");
            Debug.Log(report.ToString());
        }

        private static AimedPatternSO CreateAimedPattern()
        {
            AimedPatternSO pattern = LoadOrCreate<AimedPatternSO>($"{TempFolder}/TempPattern_Aimed.asset");
            pattern.PatternType = BulletPatternType.Aimed;
            SerializedObject so = new SerializedObject(pattern);
            so.FindProperty("_emitterIndex").intValue = 0;
            so.FindProperty("_burstCount").intValue = 5;
            so.FindProperty("_burstInterval").floatValue = 0.2f;
            so.FindProperty("_bulletSpeed").floatValue = 9f;
            so.FindProperty("_damage").floatValue = 10f;
            so.FindProperty("_lifeTime").floatValue = 5f;
            so.FindProperty("_isHoming").boolValue = false;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(pattern);
            return pattern;
        }

        private static RingPatternSO CreateRingPattern()
        {
            RingPatternSO pattern = LoadOrCreate<RingPatternSO>($"{TempFolder}/TempPattern_Ring.asset");
            pattern.PatternType = BulletPatternType.Ring;
            SerializedObject so = new SerializedObject(pattern);
            so.FindProperty("_emitterIndex").intValue = 0;
            so.FindProperty("_bulletCount").intValue = 12;
            so.FindProperty("_bulletSpeed").floatValue = 5f;
            so.FindProperty("_damage").floatValue = 10f;
            so.FindProperty("_lifeTime").floatValue = 6f;
            so.FindProperty("_ringCount").intValue = 3;
            so.FindProperty("_ringInterval").floatValue = 0.5f;
            so.FindProperty("_rotationOffsetPerRing").floatValue = 15f;
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(pattern);
            return pattern;
        }

        private static BossPhaseDataSO CreatePhase(BossAttackPatternSO first, BossAttackPatternSO second, PatternSelectorSO selector)
        {
            BossPhaseDataSO phase = LoadOrCreate<BossPhaseDataSO>($"{TempFolder}/TempPhase_0.asset");
            phase.Patterns = new[]
            {
                new BossPatternEntry { pattern = first, weight = 1f, delayAfter = 1.5f },
                new BossPatternEntry { pattern = second, weight = 1f, delayAfter = 2f },
            };
            phase.Selector = selector;
            phase.NextPhaseHpThreshold = 0f;
            EditorUtility.SetDirty(phase);
            return phase;
        }

        private static BossEncounterDataSO CreateEncounterData(BossPhaseDataSO phase)
        {
            BossEncounterDataSO data = LoadOrCreate<BossEncounterDataSO>($"{TempFolder}/TempEncounter.asset");
            data.Phases = new[] { phase };
            EditorUtility.SetDirty(data);
            return data;
        }

        private static CombatScene CreateCombatScene()
        {
            CombatScene combatScene = LoadOrCreate<CombatScene>($"{TempFolder}/TempCombatScene.asset");
            combatScene.id = 0;
            EditorUtility.SetDirty(combatScene);
            return combatScene;
        }

        private static Boss SetupBoss(Scene scene, StringBuilder report)
        {
            GameObject bossObject = FindRoot(scene, BossObjectName);
            if (bossObject == null)
            {
                bossObject = new GameObject(BossObjectName);
                bossObject.transform.position = new Vector3(0f, 15f, 0f);
                Undo.RegisterCreatedObjectUndo(bossObject, "Create Temp Boss");
                report.AppendLine($"- {BossObjectName} not found, created new");
            }

            RemoveLegacyAgent(bossObject, report);

            Boss boss = bossObject.GetComponent<Boss>();
            if (boss == null)
                boss = Undo.AddComponent<Boss>(bossObject);

            BossPatternRunner runner = bossObject.GetComponent<BossPatternRunner>();
            PlayerDataSO orderData = AssetDatabase.LoadAssetAtPath<PlayerDataSO>(OrderPlayerDataPath);
            if (orderData == null)
            {
                Debug.LogError($"[TempBossSetup] Missing {OrderPlayerDataPath}");
                return null;
            }
            SerializedObject runnerSo = new SerializedObject(runner);
            runnerSo.FindProperty("_targetPlayerData").objectReferenceValue = orderData;
            runnerSo.ApplyModifiedProperties();

            ProjectileSO projectile = AssetDatabase.LoadAssetAtPath<ProjectileSO>(BossProjectilePath);
            if (projectile == null)
            {
                Debug.LogError($"[TempBossSetup] Missing {BossProjectilePath}");
                return null;
            }

            Transform emitterTransform = bossObject.transform.Find(EmitterObjectName);
            if (emitterTransform == null)
            {
                GameObject emitterObject = new GameObject(EmitterObjectName);
                Undo.RegisterCreatedObjectUndo(emitterObject, "Create Emitter");
                emitterObject.transform.SetParent(bossObject.transform, false);
                emitterTransform = emitterObject.transform;
            }

            BossProjectileEmitter emitter = emitterTransform.GetComponent<BossProjectileEmitter>();
            if (emitter == null)
                emitter = Undo.AddComponent<BossProjectileEmitter>(emitterTransform.gameObject);

            SerializedObject emitterSo = new SerializedObject(emitter);
            SerializedProperty types = emitterSo.FindProperty("_projectileTypes");
            types.arraySize = 1;
            types.GetArrayElementAtIndex(0).objectReferenceValue = projectile;
            emitterSo.ApplyModifiedProperties();

            report.AppendLine($"- Boss: {BossObjectName} (Boss + PatternRunner + Emitter[Spear])");
            return boss;
        }

        private static void RemoveLegacyAgent(GameObject bossObject, StringBuilder report)
        {
            Component[] components = bossObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null) continue;
                if (components[i].GetType().FullName != LegacyAgentTypeName) continue;

                Undo.DestroyObjectImmediate(components[i]);
                report.AppendLine("- Removed BehaviorGraphAgent from boss");
            }
        }

        private static BossEncounterController SetupEncounter(Scene scene, Boss boss, BossEncounterDataSO data)
        {
            BossEncounterController encounter = Object.FindAnyObjectByType<BossEncounterController>(FindObjectsInactive.Include);
            if (encounter == null)
            {
                GameObject encounterObject = new GameObject(EncounterObjectName);
                Undo.RegisterCreatedObjectUndo(encounterObject, "Create Boss Encounter");
                SceneManager.MoveGameObjectToScene(encounterObject, scene);
                encounter = Undo.AddComponent<BossEncounterController>(encounterObject);
            }

            SerializedObject so = new SerializedObject(encounter);
            so.FindProperty("_encounterData").objectReferenceValue = data;
            so.FindProperty("_boss").objectReferenceValue = boss;
            so.FindProperty("_dialoguePlayer").objectReferenceValue = Object.FindAnyObjectByType<DialoguePlayer>(FindObjectsInactive.Include);
            so.ApplyModifiedProperties();
            return encounter;
        }

        private static CombatManager SetupCombatManager(BossEncounterController encounter)
        {
            CombatManager combatManager = Object.FindAnyObjectByType<CombatManager>(FindObjectsInactive.Include);
            if (combatManager == null)
            {
                GameObject managerObject = new GameObject(CombatManagerObjectName);
                Undo.RegisterCreatedObjectUndo(managerObject, "Create Combat Manager");

                ManagerInitializer initializer = Object.FindAnyObjectByType<ManagerInitializer>(FindObjectsInactive.Include);
                if (initializer != null)
                    managerObject.transform.SetParent(initializer.transform, false);

                combatManager = Undo.AddComponent<CombatManager>(managerObject);
            }

            SerializedObject so = new SerializedObject(combatManager);
            so.FindProperty("_encounterController").objectReferenceValue = encounter;
            so.ApplyModifiedProperties();
            return combatManager;
        }

        private static void BindBootProfile(CombatScene combatScene, CombatManager combatManager, StringBuilder report)
        {
            BootProfile profile = AssetDatabase.LoadAssetAtPath<BootProfile>(BootProfilePath);
            if (profile == null)
            {
                report.AppendLine($"- BootProfile skipped: {BootProfilePath} not found");
                return;
            }

            profile.mode = BootMode.CombatOnly;
            profile.combatScene = combatScene;
            profile.startPhaseIndex = 0;
            profile.skipBossIntro = true;
            EditorUtility.SetDirty(profile);

            GameBootstrapper bootstrapper = Object.FindAnyObjectByType<GameBootstrapper>(FindObjectsInactive.Include);
            if (bootstrapper == null)
            {
                report.AppendLine("- GameBootstrapper not found in scene, profile not bound");
                return;
            }

            SerializedObject so = new SerializedObject(bootstrapper);
            so.FindProperty("_profile").objectReferenceValue = profile;
            so.FindProperty("_combatManager").objectReferenceValue = combatManager;
            so.FindProperty("_flowManager").objectReferenceValue = Object.FindAnyObjectByType<FlowManager>(FindObjectsInactive.Include);
            so.ApplyModifiedProperties();
            report.AppendLine("- GameBootstrapper bound: BootProfile_CombatOnly");
        }

        private static void DeleteLegacyGraph(StringBuilder report)
        {
            if (AssetDatabase.LoadMainAssetAtPath(LegacyGraphPath) == null) return;

            AssetDatabase.DeleteAsset(LegacyGraphPath);
            report.AppendLine($"- Deleted legacy BT graph: {LegacyGraphPath}");
        }

        private static GameObject FindRoot(Scene scene, string objectName)
        {
            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].name == objectName)
                    return roots[i];
            }
            return null;
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;

            int split = path.LastIndexOf('/');
            string parent = path.Substring(0, split);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, path.Substring(split + 1));
        }
    }
}
