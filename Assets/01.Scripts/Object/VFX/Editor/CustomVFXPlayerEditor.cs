using UnityEngine;
using UnityEditor;

namespace Project_Unorder.ObjectManage
{
    [CustomEditor(typeof(VFXPlayer))]
    [CanEditMultipleObjects]
    public class VFXPlayerEditor : Editor
    {
        private float _lastUpdateTime;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Play All", GUILayout.Height(35)))
            {
                _lastUpdateTime = (float)EditorApplication.timeSinceStartup;
                
                foreach (var obj in targets)
                {
                    VFXPlayer player = (VFXPlayer)obj;
                    player.Play();
                }

                EditorApplication.update -= UpdateEditor;
                EditorApplication.update += UpdateEditor;
            }

            if (GUILayout.Button("Stop All", GUILayout.Height(35)))
            {
                foreach (var obj in targets)
                {
                    VFXPlayer player = (VFXPlayer)obj;
                    player.Stop();
                }
                EditorApplication.update -= UpdateEditor;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void UpdateEditor()
        {
            if (Application.isPlaying) return;

            float deltaTime = (float)EditorApplication.timeSinceStartup - _lastUpdateTime;
            _lastUpdateTime = (float)EditorApplication.timeSinceStartup;

            bool anyPlaying = false;

            foreach (var obj in targets)
            {
                VFXPlayer player = (VFXPlayer)obj;
                if (player == null) continue;

                var ps = player.GetComponentInChildren<ParticleSystem>();
                if (ps != null && ps.IsAlive())
                {
                    ps.Simulate(deltaTime, true, false, false);
                    anyPlaying = true;
                }

                EditorUtility.SetDirty(player);
            }

            if (!anyPlaying)
            {
                // EditorApplication.update -= UpdateEditor;
            }

            SceneView.RepaintAll();
        }
    }
}