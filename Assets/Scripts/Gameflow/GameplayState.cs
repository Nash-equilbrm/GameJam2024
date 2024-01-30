using Tools;
using HaloKero.UI.Overlap;
using HaloKero.UI.Popup;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Realtime;
using Photon.Pun;
using HaloKero.UI;
using ExitGames.Client.Photon;
using System.Collections;


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
            UIManager.Instance?.ShowOverlap<GameplayOverlap>(forceShowData: true);


            _context.Register(EventID.EndGamePlay, OnEndGame);
            _context.Register(EventID.BackToMenu, GoBackToMainMenu);


            ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable() { { "canJoinRoom", false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
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
            if(_timer > 0)
            {
                _timer -= Time.deltaTime;
                _context.Broadcast(EventID.OnTimeChanged, _timer);
            }
            else if(_playing) 
            {
                Debug.Log("time up");
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

            Debug.Log("Show screen");
            UIManager.Instance?.ShowScreen<ResultScreen>(forceShowData: true);
            _context.StartCoroutine(OnTimeUpCoRoutine());
        }

        private void OnEndGame(object data)
        {
            EventID result = (EventID)data;
            // ... do something
        }




        private float _checkForResultDelay = 1f;
        private float _checkResultTimer = 0f;
        private IEnumerator OnTimeUpCoRoutine()
        {
            while(_checkResultTimer < _checkForResultDelay)
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
            foreach (var p in PhotonNetwork.PlayerList)
            {
                if (p.CustomProperties.TryGetValue("p" + p.ActorNumber.ToString(), out object scoreObj))
                {
                    float score = (float)scoreObj;
                    if (max < score)
                    {
                        max = score;
                        winnerActorNumber = p.ActorNumber;
                    }
                }
                else
                {
                    // Handle the case where "ready" custom property is not found
                    Debug.LogWarning("Custom property 'ready' not found for player: " + p.ActorNumber);
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

