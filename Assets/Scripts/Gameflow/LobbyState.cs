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
using HaloKero.Lobby;



namespace HaloKero.Gameplay
{
    public class LobbyState : State<GameflowManager>
    {

        public LobbyState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            GameObject obj = PhotonNetwork.Instantiate(_context.playerLobbyRegisterPrefab.name, Vector3.zero, Quaternion.identity);
            _context.PlayerLobbyRegisters.Add(obj.GetComponent<PlayerLobbyRegister>());

            UIManager.Instance.HideAllScreens();
            UIManager.Instance.HideAllOverlaps ();
            UIManager.Instance.HideAllPopups();
            UIManager.Instance.HideAllNotifies();

            UIManager.Instance.ShowScreen<LobbyScreen>(forceShowData: true);
            _context.Register(EventID.OnPlayerEnter, _context.GetAllLobbyRegister);
            _context.Register(EventID.StartGamePlay, _context.CheckStartGameRequirements);


            foreach (var p in PhotonNetwork.PlayerList)
            {
                _context.Broadcast(EventID.SetPlayerID, p.ActorNumber);
            }




        }

        public override void Exit()
        {
            _context.Unregister(EventID.OnPlayerEnter, _context.GetAllLobbyRegister);
            _context.Unregister(EventID.StartGamePlay, _context.CheckStartGameRequirements);
        }

        public override void LogicUpdate()
        {
            foreach (var register in _context.PlayerLobbyRegisters)
            {
                if (!register.IsReady)
                {
                    return;
                }
            }

            // go to new scene
            UIManager.Instance.HideAllScreens();
            GameflowManager.Instance.ChangeState(GameFlowState.Gameplay);
        }
    }
}

