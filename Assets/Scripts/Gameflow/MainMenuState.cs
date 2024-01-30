using Tools;
using HaloKero.UI.Screen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using HaloKero.UI.Popup;
using Photon.Pun;
using HaloKero.UI;


namespace HaloKero.Gameplay
{
    public class MainMenuState : State<GameflowManager>
    {
        private bool _firstConnectAttempt = true;

        public MainMenuState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            _timer = 0f;

            UIManager.Instance?.HideAllScreens();
            UIManager.Instance?.HideAllOverlaps();
            UIManager.Instance?.HideAllPopups();

            _context.Register(EventID.OnConnectToServerSuccess, OnConnectToServerSuccess);
            _context.Register(EventID.OnJoinRoomSuccess, LoadLobbyScene);
            _context.Register(EventID.OnJoinRoomFailed, OnJoinRoomFailed);

            InitApp();
        }

        public override void Exit()
        {
            _context.Unregister(EventID.OnConnectToServerSuccess, OnConnectToServerSuccess);
            _context.Unregister(EventID.OnJoinRoomSuccess, LoadLobbyScene);
            _context.Unregister(EventID.OnJoinRoomFailed, OnJoinRoomFailed);

        }

        private void OnConnectToServerSuccess(object data = null)
        {
            if (_firstConnectAttempt)
            {
                UIManager.Instance?.ShowPopup<CreateOrJoinRoomPopup>(forceShowData: true);
                _firstConnectAttempt = false;
            }

        }

        private void OnConnectToServerFailed(object data = null)
        {
            _firstConnectAttempt = false;
            UIManager.Instance?.ShowPopup<WarningPopup>(forceShowData: true);
        }


        private void InitApp()
        {
            UIManager.Instance?.HideAllScreens();
            UIManager.Instance?.HideAllOverlaps();
            UIManager.Instance?.HideAllPopups();
            UIManager.Instance?.HideAllNotifies();


            UIManager.Instance?.ShowScreen<MainMenuScreen>(forceShowData: true);
        }

        private void LoadLobbyScene(object data = null)
        {
            Debug.Log("LoadGameplayScene");
            PhotonNetwork.LoadLevel(1);
            _context.ChangeState(GameFlowState.Lobby);
        }

        private void OnJoinRoomFailed(object data) {
            UIManager.Instance?.ShowPopup<WarningPopup>((string)data, forceShowData: true);
        }


        #region anim
        private float _fakeLoadDuration = 1f;
        private float _timer = 0f;
        private IEnumerator LoadAsync(int scenceIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(scenceIndex);

            float prog = 0;
            while (!operation.isDone)
            {
                prog = operation.progress;
                _context.Broadcast(EventID.OnGameLoading, prog);
                yield return null;
            }
            while (_timer < _fakeLoadDuration)
            {
                prog = Mathf.Lerp(.9f, 1, _timer / _fakeLoadDuration);
                _context.Broadcast(EventID.OnGameLoading, prog);
                _timer += Time.deltaTime;
                yield return null;
            }
            _context.Broadcast(EventID.OnGameLoading, 1f);
            _context.ChangeState(GameFlowState.Lobby);
        }

        #endregion
    }
}

