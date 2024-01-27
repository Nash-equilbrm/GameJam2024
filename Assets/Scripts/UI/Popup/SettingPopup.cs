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
        [SerializeField] private TMP_Text _settingTitleTxt;
        [SerializeField] private TMP_Text _musicSettingTitleTxt;
        [SerializeField] private TMP_Text _soundFxSettingTitleTxt;
        [SerializeField] private TMP_Text _languageSettingTitleTxt;

        [SerializeField] private Button _exitBtn;
        [SerializeField] private TMP_Text _languageSettingTxt;
        [SerializeField] private Button _languageSettingLeftBtn;
        [SerializeField] private Button _languageSettingRightBtn;
        [SerializeField] private Slider _musicSettingSlider;
        [SerializeField] private Slider _soundFXSettingSlider;


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

            _exitBtn.onClick.AddListener(Hide);
            _languageSettingLeftBtn.onClick.AddListener(OnLanguageSettingSwitchLeft);
            _languageSettingRightBtn.onClick.AddListener(OnLanguageSettingSwitchRight);
            _musicSettingSlider.onValueChanged.AddListener(SetMusic);
            _soundFXSettingSlider.onValueChanged.AddListener(SetSoundFx);
            this.Register(EventID.OnLanguageChange, Relocalize);

            _timer = 0f;
            Relocalize();
            GameSettings gameSettings = (GameSettings)data;
            _musicSettingSlider.value = gameSettings.Music;
            _soundFXSettingSlider.value = gameSettings.SoundFx;
            StartCoroutine(ShowPopup());
        }

        public override void Hide()
        {
            _exitBtn.onClick.RemoveListener(Hide);
            _languageSettingLeftBtn.onClick.RemoveListener(OnLanguageSettingSwitchLeft);
            _languageSettingRightBtn.onClick.RemoveListener(OnLanguageSettingSwitchRight);
            _musicSettingSlider.onValueChanged.RemoveListener(SetMusic);
            _soundFXSettingSlider.onValueChanged.RemoveListener(SetSoundFx);
            this.Unregister(EventID.OnLanguageChange, Relocalize);


            _timer = 0f;
            StartCoroutine(HidePopup());
        }


        private void OnLanguageSettingSwitchLeft()
        {
            GameSettingManager.Instance.SetNewSettings(increaseLanguageIdx: -1);
            this.Broadcast(EventID.OnLanguageChange);
        }

        private void OnLanguageSettingSwitchRight()
        {
            GameSettingManager.Instance.SetNewSettings(increaseLanguageIdx: 1);
            this.Broadcast(EventID.OnLanguageChange);
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
            GameSettingManager.Instance.SetNewSettings(music: value);
        }

        private void SetSoundFx(float value)
        {
            GameSettingManager.Instance.SetNewSettings(soundFx: value);
        }


        private void Relocalize(object data = null)
        {
            _settingTitleTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.SETTING_TITLE;
            _languageSettingTitleTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.LANGUAGE_SETTING_TITLE;
            _musicSettingTitleTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.MUSIC_SETTING_TITLE;
            _soundFxSettingTitleTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.SOUND_FX_SETTING_TITLE;
            _languageSettingTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.LANGUAGE_ID;

        }

    }
}

