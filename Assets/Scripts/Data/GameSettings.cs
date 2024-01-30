using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace HaloKero.Settings
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Scriptable Objects/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField][Range(0f, 1f)] private float _music = 0.5f;
        [SerializeField][Range(0f, 1f)] private float _soundFx = 0.5f;
        [SerializeField] private float _gameDuration = 1200;
        public float Music { get => _music; }
        public float SoundFx { get => _soundFx; }
        public float GameDuration { get => _gameDuration; }

        public void SetNewSettings(float music, float soundFx)
        {
            _music = (music < 0f) ? Music : music;
            _soundFx = (soundFx < 0f) ? SoundFx : soundFx;
        }

    }
}
