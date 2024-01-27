using HaloKero.Multiplayer;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HaloKero.Lobby
{
    public class PlayerLobbyRegister : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private bool _isReady;
        public bool IsReady { get => _isReady; }

        private void Start()
        {
            DontDestroyOnLoad(this);
            this.Register(EventID.StartGamePlay, SetPlayerReady);
            DEBUG();
        }


        private void OnDestroy()
        {
            this.Unregister(EventID.StartGamePlay, SetPlayerReady);
        }

        private void SetPlayerReady(object IsReady = null)
        {
            NetworkManager.CallRPC(_photonView, "SetPlayerReady_RPC", data: !_isReady, onlyForLocalPlayer: true);
        }


        [PunRPC]
        private void SetPlayerReady_RPC(bool isReady)
        {
            _isReady = isReady;
        }

        private void DEBUG()
        {
            NetworkManager.CallRPC(_photonView, "DEBUG_RPC", onlyForLocalPlayer: false);
        }


        [PunRPC]
        private void DEBUG_RPC()
        {
            Debug.Log("PLAYER CREATE");
        }
    }
}

