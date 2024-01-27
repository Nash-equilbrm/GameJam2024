using Tools;
using HaloKero.UI.Overlap;
using HaloKero.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HaloKero.Gameplay
{
    public class GameplayState : State<GameflowManager>
    {
        private float _gameDuration = 65;
        private float _timer;
        public GameplayState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            _timer = _gameDuration;
            UIManager.Instance.HideAllScreens();
            UIManager.Instance.ShowOverlap<GameplayOverlap>(forceShowData: true);

            _context.Register(EventID.EndGamePlay, OnEndGame);
            _context.Register(EventID.OpenMainMenu, GoBackToMainMenu);
        }

        public override void Exit()
        {
            _animTimer = 0;

            // Save game result
            //...


            // Unregister Game events
            _context.Unregister(EventID.EndGamePlay, OnEndGame);
            _context.Unregister(EventID.OpenMainMenu, GoBackToMainMenu);
        }

        public override void LogicUpdate()
        {
            if(_timer > 0)
            {
                _timer -= Time.deltaTime;
                _context.Broadcast(EventID.OnTimeChanged, _timer);
            }
            else
            {
                _context.Broadcast(EventID.TimeUp);
            }
        }

        private void OnEndGame(object data)
        {
            EventID result = (EventID)data;
            _context.StartCoroutine(OnEndGameCoroutine());
        }

        private float _showResultPopupDuration = 1.5f;
        private float _animTimer = 0f;
        private IEnumerator OnEndGameCoroutine()
        {
            while(_animTimer < _showResultPopupDuration)
            {
                _animTimer += Time.deltaTime;
                yield return null;
            }
            UIManager.Instance.ShowPopup<AboutUsPopup>(forceShowData: true);
        }


        private void GoBackToMainMenu(object data)
        {
            _context.ChangeState(GameFlowState.MainMenu);
        }
    }
}

