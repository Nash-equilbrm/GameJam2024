using HaloKero.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace HaloKero.UI.Popup
{
    public class FinalWordsPopup : BasePopup
    {
        [Header("Widget")]
        [SerializeField] private GameObject _popup;
        [SerializeField] private TMP_Text _finishGameBtnTxt;
        [SerializeField] private Button _finishGameBtn;
        [SerializeField] private Button _exitPopUpBtn;


        [Header("Show pop up anim")]
        [SerializeField] float _popUpShowDuration = 0.5f;
        [SerializeField] float _popUpShowStartScale = 0.5f;
        [SerializeField] float _popUpShowEndScale = 1f;

        [Header("Hide pop up anim")]
        [SerializeField] float _popUpHideDuration = 0.5f;
        [SerializeField] float _popUpHideStartScale = 1f;
        [SerializeField] float _popUpHideEndScale = 1f;


        private float _timer = 0f;

        public override void Hide()
        {
            _finishGameBtn.onClick.RemoveListener(BackToMenu);
            _exitPopUpBtn.onClick.RemoveListener(BackToMenu);
            this.Unregister(EventID.OnLanguageChange, Relocalize);

            _timer = 0f;
            StartCoroutine(HidePopup());
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            _finishGameBtn.onClick.AddListener(BackToMenu);
            _exitPopUpBtn.onClick.AddListener(BackToMenu);
            this.Register(EventID.OnLanguageChange, Relocalize);

            Relocalize();

            _timer = 0f;
            StartCoroutine(ShowPopup());
        }


        private void BackToMenu()
        {
            this.Broadcast(EventID.OpenMainMenu);
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


        private void Relocalize(object data = null)
        {
            _finishGameBtnTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.FINISH_GAME_BTN;
        }
    }

}
