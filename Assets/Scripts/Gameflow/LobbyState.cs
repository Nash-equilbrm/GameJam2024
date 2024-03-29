using Tools;
using HaloKero.UI.Overlap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using HaloKero.Multiplayer;
using Unity.VisualScripting;
using HaloKero.UI;
using ExitGames.Client.Photon;
using HaloKero.UI.Popup;



namespace HaloKero.Gameplay
{
    public class LobbyState : State<GameflowManager>
    {
   
        public LobbyState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            Debug.Log("Create player");
            UIManager.Instance?.HideAllScreens();
            UIManager.Instance?.HideAllOverlaps ();
            UIManager.Instance?.HideAllPopups();
            UIManager.Instance?.HideAllNotifies();

            UIManager.Instance?.ShowScreen<LobbyScreen>(forceShowData: true);
          

            _context.Register(EventID.StartGamePlay, StartGameplay);
            _context.Register(EventID.BackToMenu, GoBackToMainMenu);
            _context.Broadcast(EventID.OnPlayerEnter);
        }


        private void StartGameplay(object data = null)
        {
            UIManager.Instance?.HideAllScreens();
            GameflowManager.Instance?.ChangeState(GameFlowState.Gameplay);
        }

        public override void Exit()
        {
            _context.Unregister(EventID.StartGamePlay, StartGameplay);
            _context.Unregister(EventID.BackToMenu, GoBackToMainMenu);

        }

        public override void LogicUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance?.ShowPopup<SettingPopup>(data: GameSettingManager.Instance?.CurrentSettings, forceShowData: true);
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

