using HaloKero.Gameplay;
using HaloKero.Settings;
using HaloKero.UI.Overlap;
using HaloKero.UI.Screen;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace HaloKero.UI.Popup
{
    public class SettingPopup : BasePopup
    {
        [Header("Widgets")]
        [SerializeField] private GameObject _popup;
        [SerializeField] private TMP_Text _musicSettingTitleTxt;
        [SerializeField] private TMP_Text _soundFxSettingTitleTxt;
        [SerializeField] private Button _exitBtn;
        [SerializeField] private Button _exitGameBtn;
        [SerializeField] private Slider _musicSettingSlider;
        [SerializeField] private Slider _soundFXSettingSlider;
        [SerializeField] private Button _aboutUsBtn;


        [Header("Show pop up anim")]
        [SerializeField] float _popUpShowDuration = 0.5f;
        [SerializeField] float _popUpShowStartScale = 0.5f;
        [SerializeField] float _popUpShowEndScale = 1f;

        [Header("Hide pop up anim")]
        [SerializeField] float _popUpHideDuration = 0.5f;
        [SerializeField] float _popUpHideStartScale = 1f;
        [SerializeField] float _popUpHideEndScale = 1f;
        private float _timer = 0f;

        public override void Init()
        {
            base.Init();
           
        }

        public override void Show(object data)
        {
            base.Show(null);
            this.Broadcast(EventID.OnBtnClick);

            _exitBtn.onClick.AddListener(ExitOnClick);
            _exitGameBtn.onClick.AddListener(ExitAppOnClick);
            _musicSettingSlider.onValueChanged.AddListener(SetMusic);
            _soundFXSettingSlider.onValueChanged.AddListener(SetSoundFx);
            _aboutUsBtn.onClick.AddListener(ShowAboutUsPopupOnClick);


            _timer = 0f;
            GameSettings gameSettings = (GameSettings)data;
            _musicSettingSlider.value = gameSettings.Music;
            _soundFXSettingSlider.value = gameSettings.SoundFx;
            StartCoroutine(ShowPopup());
        }

        public override void Hide()
        {
            _exitBtn.onClick.RemoveListener(ExitOnClick);
            _exitGameBtn.onClick.RemoveListener(ExitAppOnClick);
            _musicSettingSlider.onValueChanged.RemoveListener(SetMusic);
            _soundFXSettingSlider.onValueChanged.RemoveListener(SetSoundFx);
            _aboutUsBtn.onClick.RemoveListener(ShowAboutUsPopupOnClick);



            _timer = 0f;
            StartCoroutine(HidePopup());
        }

        private void ShowAboutUsPopupOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);
            UIManager.Instance?.ShowPopup<AboutUsPopup>(forceShowData: true);
        }
        private void ExitAppOnClick()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                ExitOnClick();
            }
            else
            {
                this.Broadcast(EventID.OnBtnClick);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private void ExitOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);
            Hide();
        }

        private IEnumerator ShowPopup()
        {
            while (_timer < _popUpShowDuration)
            {
                float localScale = Mathf.Lerp(_popUpShowStartScale, _popUpShowEndScale, _timer / _popUpShowDuration);
                _popup.transform.localScale = new Vector3(localScale, localScale, localScale);
                _timer += Time.deltaTime;
                yield return null;
            }
            _popup.transform.localScale = new Vector3(_popUpShowEndScale, _popUpShowEndScale, _popUpShowEndScale);
            _timer = 0f;
        }

        private IEnumerator HidePopup()
        {
            while (_timer < _popUpHideDuration)
            {
                float localScale = Mathf.Lerp(_popUpHideStartScale, _popUpHideEndScale, _timer / _popUpHideDuration);
                _popup.transform.localScale = new Vector3(localScale, localScale, localScale);
                _timer += Time.deltaTime;
                yield return null;
            }
            _popup.transform.localScale = new Vector3(_popUpHideEndScale, _popUpHideEndScale, _popUpHideEndScale);
            _timer = 0f;
            base.Hide();
        }


        private void SetMusic(float value)
        {
            this.Broadcast(EventID.OnMusicVolumeChanged, value);
            GameSettingManager.Instance?.SetNewSettings(music: value);
        }

        private void SetSoundFx(float value)
        {
            this.Broadcast(EventID.OnSFXVolumeChanged, value);
            GameSettingManager.Instance?.SetNewSettings(soundFx: value);
        }


    }
}

