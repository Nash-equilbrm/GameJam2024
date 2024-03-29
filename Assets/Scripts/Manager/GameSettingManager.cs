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


        private void Start()
        {
            //set up data
            this.Broadcast(EventID.OnMusicVolumeChanged, _settings.Music);
            this.Broadcast(EventID.OnSFXVolumeChanged, _settings.SoundFx);

        }

        public void SetNewSettings(float music = -1f, float soundFx = -1f)
        {
            _settings.SetNewSettings(music, soundFx);
        }
    }
}

