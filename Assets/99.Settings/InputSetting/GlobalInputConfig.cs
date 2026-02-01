using UnityEngine;
namespace Project_Unorder.InputManage
{

    public enum InputConfigPresetEnum
    {
        AllEnabled,
        AllDisabled,
        DialogueMode
    }
    public static class GlobalInputConfig
    {
        // ================================================================
        // Player Input Configs
        // ================================================================
        public static bool PLAYER_MOVE = true;
        public static bool PLAYER_INTERACT = true;
        public static bool PLAYER_CLICK = true;
        public static bool PLAYER_DASH = true;

        // ================================================================
        // Player Input Configs
        // ================================================================

        public static bool UI_SUBMIT = true;
        public static bool UI_CANCEL = true;



        public static void EnableAll()
        {
            PLAYER_MOVE = true;
            PLAYER_INTERACT = true;
            PLAYER_CLICK = true;
            PLAYER_DASH = true;

            UI_SUBMIT = true;
            UI_CANCEL = true;
        }

        public static void DisableAll()
        {
            PLAYER_MOVE = false;
            PLAYER_INTERACT = false;
            PLAYER_CLICK = false;
            PLAYER_DASH = false;

            UI_SUBMIT = false;
            UI_CANCEL = false;
        }

        // ================================================================
        // QUICK setting Functions
        // ================================================================

        public static void SetPreset(InputConfigPresetEnum preset)
        {
            switch (preset)
            {
                case InputConfigPresetEnum.AllEnabled:
                    EnableAll();
                    break;
                case InputConfigPresetEnum.AllDisabled:
                    DisableAll();
                    break;
                case InputConfigPresetEnum.DialogueMode:
                    SetPresetDialogueMode();
                    break;
            }
        }

        public static void SetPresetDialogueMode()
        {
            PLAYER_MOVE = false;
            PLAYER_INTERACT = false;
            PLAYER_CLICK = false;
            PLAYER_DASH = false;

            UI_SUBMIT = true;
            UI_CANCEL = true;
        }



    }

}