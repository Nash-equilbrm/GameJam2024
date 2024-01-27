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
        public GameplayState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            UIManager.Instance.HideAllScreens();
            UIManager.Instance.ShowOverlap<GameplayOverlap>(forceShowData: true);

            _context.Register(EventID.EndGamePlay, (data) => OnEndGame((EventID)data));
            _context.Register(EventID.OpenMainMenu, (data) => GoBackToMainMenu());
        }

        public override void Exit()
        {
            _timer = 0;

            // Save game result
            //...


            // Unregister Game events
            _context.Unregister(EventID.EndGamePlay, (data) => OnEndGame((EventID)data));
            _context.Unregister(EventID.OpenMainMenu, (data) => GoBackToMainMenu());
        }

        private void OnEndGame(EventID result)
        {
            _context.StartCoroutine(OnEndGameCoroutine());
        }

        private float _showResultPopupDuration = 1.5f;
        private float _timer = 0f;
        private IEnumerator OnEndGameCoroutine()
        {
            while(_timer < _showResultPopupDuration)
            {
                _timer += Time.deltaTime;
                yield return null;
            }
            UIManager.Instance.ShowPopup<FinalWordsPopup>(forceShowData: true);
        }


        private void GoBackToMainMenu()
        {
            _context.ChangeState(GameFlowState.MainMenu);
        }
    }
}

