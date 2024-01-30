using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace HaloKero.UI.Popup
{
    public class CreateOrJoinRoomPopup : BasePopup
    {
        [Header("Widgets")]
        [SerializeField] private GameObject _popup;
        [SerializeField] private Button _createGameBtn;
        [SerializeField] private TMP_InputField _createGameInputField;
        [SerializeField] private Button _joinGameBtn;
        [SerializeField] private TMP_InputField _joinGameInputField;
        [SerializeField] private Button _exitBtn;


        public override void Hide()
        {
            base.Hide();
            _createGameBtn.onClick.RemoveListener(CreateGame);
            _joinGameBtn.onClick.RemoveListener(JoinGame);
            _exitBtn.onClick.RemoveListener(Hide);

            this.Unregister(EventID.OnJoinRoomSuccess, OnJoinRoomSuccess);

        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            _createGameBtn.onClick.AddListener(CreateGame);
            _joinGameBtn.onClick.AddListener(JoinGame);
            _exitBtn.onClick.AddListener(Hide);
            this.Broadcast(EventID.OnPopupShow);

            this.Register(EventID.OnJoinRoomSuccess, OnJoinRoomSuccess);

        }

        private void JoinGame()
        {
            this.Broadcast(EventID.OnJoinRoom, _joinGameInputField.text);
        }

        private void CreateGame()
        {
            this.Broadcast(EventID.OnCreateRoom, _createGameInputField.text);
        }

        private void OnJoinRoomSuccess(object data = null)
        {
            Hide();
        }


        #region anim
        [Header("Show pop up anim")]
        [SerializeField] float _popUpShowDuration = 0.5f;
        [SerializeField] float _popUpShowStartScale = 0.5f;
        [SerializeField] float _popUpShowEndScale = 1f;

        [Header("Hide pop up anim")]
        [SerializeField] float _popUpHideDuration = 0.5f;
        [SerializeField] float _popUpHideStartScale = 1f;
        [SerializeField] float _popUpHideEndScale = 1f;
        private float _timer = 0f;

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
        #endregion
    }
}

