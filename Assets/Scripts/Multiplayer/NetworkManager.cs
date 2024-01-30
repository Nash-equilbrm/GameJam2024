using ExitGames.Client.Photon;
using HaloKero.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace HaloKero.Multiplayer
{
    public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private int _maxPlayerPerRoom = 4;

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
            this.Unregister(EventID.OnCreateRoom, CreateRoom);
            this.Unregister(EventID.OnJoinRoom, JoinRoom);
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

        public override void OnDisconnected(DisconnectCause cause)
        {
            this.Broadcast(EventID.OnJoinRoomFailed, "connection error \n please check your internet connection and try again");
        }



        public override void OnJoinedLobby()
        {
            this.Broadcast(EventID.OnConnectToServerSuccess);
        }


        public void CreateRoom(object data)
        {
            PhotonNetwork.CreateRoom(data as string);
        }

        public override void OnCreatedRoom()
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(
                new Hashtable
                {
                    { "canJoinRoom", true }

                }
            );
        }


        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (!PhotonNetwork.IsConnected)
            {
                this.Broadcast(EventID.OnJoinRoomFailed, "connection error \n please check your internet connection and try again");
            }
            else
            {
                this.Broadcast(EventID.OnJoinRoomFailed, "there is already a room with this id \n please check your internet connection and try again");
            }
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
            if(PhotonNetwork.PlayerList.Count() >= _maxPlayerPerRoom + 1)
            {
                PhotonNetwork.LeaveRoom();
                this.Broadcast(EventID.OnJoinRoomFailed, "this room is full. \n please choose another room!");
                return;
            }

            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("canJoinRoom", out object obj))
            {
                bool canJoin = (bool)obj;
                if (!canJoin)
                {
                    PhotonNetwork.LeaveRoom();
                    this.Broadcast(EventID.OnJoinRoomFailed, "the game has already started \n please find another room");
                    return;
                }
            }

            this.Broadcast(EventID.OnJoinRoomSuccess);
            Debug.Log("player join: " + PhotonNetwork.PlayerList.Count());
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            this.Broadcast(EventID.OnJoinRoomFailed, "connection error \n please check your internet connection and try again");
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {

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

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == (int)EventID.StartGamePlay)
            {
                this.Broadcast(EventID.StartGamePlay);
            }
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel(0);
            if (GameflowManager.Instance.Player)
            {
                PhotonNetwork.Destroy(GameflowManager.Instance.Player);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom");
            this.Broadcast(EventID.OnPlayerLeft, otherPlayer);
        }

        private void Update()
        {
            
            
        }

    }
}

