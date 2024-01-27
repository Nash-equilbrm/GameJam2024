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
            //GameObject obj = PhotonNetwork.Instantiate(_context.playerLobbyRegisterPrefab.name, Vector3.zero, Quaternion.identity);
            //_context.PlayerLobbyRegisters.Add(obj.GetComponent<PlayerLobbyRegister>());

            UIManager.Instance.HideAllScreens();
            UIManager.Instance.HideAllOverlaps ();
            UIManager.Instance.HideAllPopups();
            UIManager.Instance.HideAllNotifies();

            UIManager.Instance.ShowScreen<LobbyScreen>(forceShowData: true);
          


            foreach (var p in PhotonNetwork.PlayerList)
            {
                _context.Broadcast(EventID.SetPlayerID, p.ActorNumber);
            }

            _context.Register(EventID.StartGamePlay, StartGameplay);
        }


        private void StartGameplay(object data = null)
        {
            UIManager.Instance.HideAllScreens();
            GameflowManager.Instance.ChangeState(GameFlowState.Gameplay);
        }

        public override void Exit()
        {
            _context.Unregister(EventID.StartGamePlay, StartGameplay);
        }



    }
}

