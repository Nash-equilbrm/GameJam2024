using Tools;
using HaloKero.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HaloKero.Gameplay
{
    public class GameSettingManager : Singleton<GameSettingManager>
    {
        [SerializeField] private GameSettings _settings;
        public GameSettings CurrentSettings { get => _settings; }

        public void SetNewSettings(int increaseLanguageIdx = 0, float music = -1f, float soundFx = -1f)
        {
            _settings.SetNewSettings(increaseLanguageIdx, music, soundFx);
        }
    }
}

