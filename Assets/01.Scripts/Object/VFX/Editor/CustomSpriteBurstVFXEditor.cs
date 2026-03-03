using UnityEngine;
using UnityEditor;

namespace Project_Unorder.ObjectManage.VFX
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SpriteBurstVFX))]
    public class SpriteBurstVFXEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SpriteBurstVFX vfx = (SpriteBurstVFX)target;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Play", GUILayout.Height(30)))
            {
                if (!Application.isPlaying)
                {
                    EditorApplication.update -= UpdateEditor;
                    EditorApplication.update += UpdateEditor;
                }
                vfx.Play();
            }

            if (GUILayout.Button("Stop", GUILayout.Height(30)))
            {
                vfx.Stop();
                EditorApplication.update -= UpdateEditor;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void UpdateEditor()
        {
            SpriteBurstVFX vfx = (SpriteBurstVFX)target;
            if (vfx == null)
            {
                EditorApplication.update -= UpdateEditor;
                return;
            }

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(vfx);
                EditorUtility.SetDirty(vfx.transform);
                SceneView.RepaintAll();
            }
        }
    }
}