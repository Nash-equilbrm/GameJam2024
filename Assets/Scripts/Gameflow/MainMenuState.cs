using Tools;
using HaloKero.UI.Screen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using HaloKero.UI.Popup;


namespace HaloKero.Gameplay
{
    public class MainMenuState : State<GameflowManager>
    {
        public MainMenuState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            _timer = 0f;
            _context.Register(EventID.OnConnectToServerSuccess, OpenCreateOrJoinRoomPopup);
            _context.Register(EventID.OnJoinRoomSuccess, LoadLobbyScene);
            InitApp();
        }

        public override void Exit()
        {
            _context.Unregister(EventID.OnConnectToServerSuccess, OpenCreateOrJoinRoomPopup);
            _context.Unregister(EventID.OnJoinRoomSuccess, LoadLobbyScene);
        }

        private void OpenCreateOrJoinRoomPopup(object data = null)
        {
            UIManager.Instance.ShowPopup<CreateOrJoinRoomPopup>(forceShowData: true);
        }


        private void InitApp()
        {
            UIManager.Instance.HideAllScreens();
            UIManager.Instance.HideAllOverlaps();
            UIManager.Instance.HideAllPopups();
            UIManager.Instance.HideAllNotifies();

            if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            {
                SceneManager.UnloadSceneAsync(1);
            }
            UIManager.Instance.ShowScreen<MainMenuScreen>(forceShowData: true);
        }

        private void LoadLobbyScene(object data = null)
        {
            UIManager.Instance.ShowScreen<LoadingScreen>(forceShowData: true);
            _context.StartCoroutine(LoadAsync(1));
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

