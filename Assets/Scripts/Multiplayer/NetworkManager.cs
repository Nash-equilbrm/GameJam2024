using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace HaloKero.Multiplayer
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            this.Register(EventID.OnConnectToServer, ConnectToServer);
            this.Register(EventID.OnCreateRoom, CreateRoom);
            this.Register(EventID.OnJoinRoom, JoinRoom);
        }

        private void OnDestroy()
        {
            this.Unregister(EventID.OnConnectToServer, ConnectToServer);
        }



        private void ConnectToServer(object data = null)
        {
            if(PhotonNetwork.IsConnected)
            {
                this.Broadcast(EventID.OnConnectToServerSuccess);
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }

        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }


        public override void OnJoinedLobby()
        {
            this.Broadcast(EventID.OnConnectToServerSuccess);
        }



        public void CreateRoom(object data)
        {
            PhotonNetwork.CreateRoom(data as string);
        }


        public void JoinRoom(object data)
        {
            PhotonNetwork.JoinRoom(data as string);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
        }


        public override void OnJoinedRoom()
        {
            this.Broadcast(EventID.OnJoinRoomSuccess);
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed");
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("OnPlayerEnteredRoom");

            this.Broadcast(EventID.OnPlayerEnter, newPlayer.ActorNumber);
        }


        public static void CallRPC(PhotonView photonView, string callback, object data = null, bool onlyForLocalPlayer = false)
        {
            if (onlyForLocalPlayer && photonView.IsMine || !onlyForLocalPlayer)
            {
                PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, callback);
                photonView.RPC(callback, RpcTarget.AllBuffered, data);
                PhotonNetwork.SendAllOutgoingCommands();
            }
        }

    }
}

