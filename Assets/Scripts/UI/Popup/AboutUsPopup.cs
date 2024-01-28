using HaloKero.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace HaloKero.UI.Popup
{
    public class AboutUsPopup : BasePopup
    {
        [Header("Widget")]
        [SerializeField] private GameObject _popup;
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
            _exitPopUpBtn.onClick.RemoveListener(Hide);

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
            _exitPopUpBtn.onClick.AddListener(Hide);


            _timer = 0f;
            StartCoroutine(ShowPopup());
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
        
    }

}
