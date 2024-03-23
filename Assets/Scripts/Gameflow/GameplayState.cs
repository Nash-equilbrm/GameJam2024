using Tools;
using HaloKero.UI.Overlap;
using HaloKero.UI.Popup;
using UnityEngine;
using System;
using Photon.Realtime;
using Photon.Pun;
using HaloKero.UI;
using ExitGames.Client.Photon;
using System.Collections;
using System.Linq;



namespace HaloKero.Gameplay
{
    public class GameplayState : State<GameflowManager>
    {
        private float _gameDuration;
        private float _timer;
        private bool _playing;

        public GameplayState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            _gameDuration = GameSettingManager.Instance.CurrentSettings.GameDuration;
            _playing = true;
            _timer = _gameDuration;
            UIManager.Instance?.HideAllScreens();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            UIManager.Instance?.ShowOverlap<GameplayOverlap>(forceShowData: true);
#elif UNITY_ANDROID
            UIManager.Instance?.ShowOverlap<GameplayOverlapTouchScreen>(forceShowData: true);
#endif
            _context.Register(EventID.EndGamePlay, OnEndGame);
            _context.Register(EventID.BackToMenu, GoBackToMainMenu);


            PhotonNetwork.CurrentRoom.SetCustomProperties(
               new ExitGames.Client.Photon.Hashtable
               {
                    { "canJoinRoom", false }

               }


           );
        }

        public override void Exit()
        {
            _checkResultTimer = 0;

            // Save game result
            //...


            // Unregister Game events
            _context.Unregister(EventID.EndGamePlay, OnEndGame);
            _context.Unregister(EventID.BackToMenu, GoBackToMainMenu);
        }

        public override void LogicUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance?.ShowPopup<SettingPopup>(data: GameSettingManager.Instance?.CurrentSettings, forceShowData: true);
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                _context.Broadcast(EventID.OnTimeChanged, _timer);
            }
            else if (_playing)
            {
                _context.Broadcast(EventID.TimeUp);
                OnTimeUp();
                _playing = false;
            }
        }

        private void OnTimeUp()
        {
            UIManager.Instance?.HideAllScreens();
            UIManager.Instance?.HideAllOverlaps();
            UIManager.Instance?.HideAllPopups();

            UIManager.Instance?.ShowScreen<ResultScreen>(forceShowData: true);

            _context.StartCoroutine(OnTimeUpCoRoutine());
        }

        private void OnEndGame(object data)
        {
            EventID result = (EventID)data;
            // ... do something
        }




        private float _checkForResultDelay = 1.5f;
        private float _checkResultTimer = 0f;
        private IEnumerator OnTimeUpCoRoutine()
        {
            while (_checkResultTimer < _checkForResultDelay)
            {
                _checkResultTimer += Time.deltaTime;
                yield return null;
            }

            CheckResult();
        }

        private void CheckResult()
        {
            Debug.Log("CheckResult");
            float max = 0;
            int winnerActorNumber = -1;
            int pCount = 0;
            while (pCount < PhotonNetwork.PlayerList.Count())
            {
                pCount = 0;
                foreach (var p in PhotonNetwork.PlayerList)
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("p" + p.ActorNumber.ToString(), out object scoreObj))
                    {
                        float score = (float)scoreObj;
                        if (max < score)
                        {
                            max = score;
                            winnerActorNumber = p.ActorNumber;
                        }
                        Debug.Log("score " + p.ActorNumber.ToString() + ": " + score);
                        pCount++;
                    }
                    else
                    {
                        // Handle the case where "ready" custom property is not found
                        Debug.LogWarning("Custom property 'score' not found for player: " + p.ActorNumber);
                    }
                }

            }

            if (PhotonNetwork.LocalPlayer.ActorNumber == winnerActorNumber)
            {
                Debug.Log("Broadcast won");
                _context.Broadcast(EventID.WonGame);
            }
            else
            {
                Debug.Log("Broadcast lost");
                _context.Broadcast(EventID.LostGame);
            }
        }


        private void GoBackToMainMenu(object data)
        {
            UIManager.Instance?.HideAllScreens();
            UIManager.Instance?.HideAllOverlaps();
            UIManager.Instance?.HideAllPopups();

            _context.ChangeState(GameFlowState.MainMenu);
        }
    }
}

