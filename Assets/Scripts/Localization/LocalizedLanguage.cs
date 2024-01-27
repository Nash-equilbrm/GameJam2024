using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Tools
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Config/Localization/Language", fileName = "New Language")]
    public class LocalizedLanguage: ScriptableObject
    {
        public string LANGUAGE_ID;
        public string SETTING_TITLE;
        public string MUSIC_SETTING_TITLE;
        public string SOUND_FX_SETTING_TITLE;
        public string LANGUAGE_SETTING_TITLE;
        public string START_GAME_BTN;
        public string FINISH_GAME_BTN;
        public string PLAYER_1_ENTER;
        public string PLAYER_2_ENTER;
        public string GET_READY_BTN;

    }
}

