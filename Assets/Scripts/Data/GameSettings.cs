using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace HaloKero.Settings
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Scriptable Objects/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        private const string EN_LANG = "en";
        private const string VIE_LANG = "vie";

        [SerializeField] private LocalizationConfig _localizationConfig;
        [SerializeField] private int _languageIdx = 0;
        [SerializeField] private float _music = 0.5f;
        [SerializeField] private float _soundFx = 0.5f;

        public int Language { get => _languageIdx; }
        public float Music { get => _music; }
        public float SoundFx { get => _soundFx; }
        public LocalizedLanguage CurrentLanguage => _localizationConfig.LocalizedLanguages[_languageIdx];

        public void SetNewSettings(int increaseLanguageIndex, float music, float soundFx)
        {
            if(increaseLanguageIndex < 0)
            {
                _languageIdx = (_languageIdx >= _localizationConfig.LocalizedLanguages.Count - 1) ? 0 : _languageIdx + 1;
            }
            else if(increaseLanguageIndex > 0)
            {
                _languageIdx = (_languageIdx <= 0) ? _localizationConfig.LocalizedLanguages.Count - 1 : _languageIdx - 1;
            }

            _music = (music < 0f) ? Music : music;
            _soundFx = (soundFx < 0f) ? SoundFx : soundFx;
        }

    }
}
