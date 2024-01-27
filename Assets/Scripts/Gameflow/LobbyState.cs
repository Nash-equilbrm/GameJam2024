using Tools;
using HaloKero.UI.Overlap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using HaloKero.Multiplayer;
using Unity.VisualScripting;



namespace HaloKero.Gameplay
{
    public class LobbyState : State<GameflowManager>
    {

        public LobbyState(GameflowManager context) : base(context)
        {
        }

        public override void Enter()
        {
            UIManager.Instance.HideAllScreens();
            UIManager.Instance.HideAllOverlaps ();
            UIManager.Instance.HideAllPopups();
            UIManager.Instance.HideAllNotifies();

            UIManager.Instance.ShowOverlap<LobbyOverlap>(forceShowData: true);


            foreach (var p in PhotonNetwork.PlayerList)
            {
                _context.Broadcast(EventID.SetPlayerID, p.ActorNumber);
            }



        }

        public override void Exit()
        {

        }

       





       

    }
}

