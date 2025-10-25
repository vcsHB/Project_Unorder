using UnityEditor;
using UnityEngine;
using MINISoundManage;
namespace MINISoundManage
{

    [InitializeOnLoad]
    public static class SoundDataIconInitializer
    {
        static SoundDataIconInitializer()
        {
            SetSoundDataIcons();
            SetSoundTableIcons();
        }

        private static void SetSoundDataIcons()
        {
            string[] guids = AssetDatabase.FindAssets("t:SoundSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SoundSO asset = AssetDatabase.LoadAssetAtPath<SoundSO>(path);

                Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/01.Scripts/SoundSystem/Editor/MINI_Sound_Icon.png");

                if (icon != null)
                {
                    EditorGUIUtility.SetIconForObject(asset, icon);
                }
            }
        }
        private static void SetSoundTableIcons()
        {
            string[] guids = AssetDatabase.FindAssets("t:SoundTableSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SoundTableSO asset = AssetDatabase.LoadAssetAtPath<SoundTableSO>(path);

                Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/01.Scripts/SoundSystem/Editor/MINI_Sound_Group_Icon.png");

                if (icon != null)
                {
                    EditorGUIUtility.SetIconForObject(asset, icon);
                }
            }
        }

    }
}
