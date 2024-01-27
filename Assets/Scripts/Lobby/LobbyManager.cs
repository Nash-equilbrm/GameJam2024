using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Tools;




namespace HaloKero.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [Header("Lobby Management")]
        public GameObject playerLobbyRegisterPrefab;
        [SerializeField] private GameObject[] _playerSlots;
        [SerializeField] private GameObject[] _playerDummyPrefabs;
        [SerializeField] private List<PlayerLobbyRegister> _playerLobbyRegisters;

        private int _localPlayerID = -1;
        private bool _gameSceneLoaded = false;
        public GameObject LocalPlayerDummyPrefabs => _playerDummyPrefabs[_localPlayerID];

        private void Start()
        {
            // create a register for player
            PhotonNetwork.Instantiate(playerLobbyRegisterPrefab.name, Vector3.zero, Quaternion.identity);
            GetAllLobbyRegister();

            this.Register(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Register(EventID.SetPlayerID, SetUpPlayerSlot);
            this.Register(EventID.OnPlayerEnter, GetAllLobbyRegister);
            this.Register(EventID.StartGamePlay, CheckStartGameRequirements);

        }

        private void OnDestroy()
        {
            this.Unregister(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Unregister(EventID.SetPlayerID, SetUpPlayerSlot);
            this.Unregister(EventID.OnPlayerEnter, GetAllLobbyRegister);
            this.Unregister(EventID.StartGamePlay,CheckStartGameRequirements);

        }



        private void Update()
        {
            foreach(var register in _playerLobbyRegisters)
            {
                if (!register.IsReady)
                {
                    return;
                }
            }

            // go to new scene
            Debug.Log("1");
            LoadGameplayScene();
        }

        private void SetUpPlayerSlot(object data)
        {
            int actorNumber = (int)data;
            _localPlayerID = actorNumber - 1;
            GameObject obj = Instantiate(_playerDummyPrefabs[_localPlayerID]);
            if (actorNumber < PhotonNetwork.LocalPlayer.ActorNumber)
            {
                obj.transform.SetParent(_playerSlots[actorNumber].transform);
            }
            else if (actorNumber > PhotonNetwork.LocalPlayer.ActorNumber)
            {
                obj.transform.SetParent(_playerSlots[actorNumber - 1].transform);
            }
            else
            {
                obj.transform.SetParent(_playerSlots[0].transform);
            }
            obj.transform.localPosition = Vector3.zero;
           
        }



        internal void GetAllLobbyRegister(object data = null)
        {
            StartCoroutine(GetAllLobbyRegisterCoroutine());
        }

        IEnumerator GetAllLobbyRegisterCoroutine()
        {
            //delay for network
            yield return new WaitForSeconds(.5f);
            _playerLobbyRegisters.Clear();
            _playerLobbyRegisters = FindObjectsOfType<PlayerLobbyRegister>().Select(x => x).ToList();
        }


        private void CheckStartGameRequirements(object data = null)
        {
            if(_playerLobbyRegisters.Count <= 1)
            {
                Debug.Log("Need more players");
                return;
            }
            foreach (var register in _playerLobbyRegisters)
            {
                if (register.photonView.IsMine && register.IsReady)
                {
                    Debug.Log("Player set unready");
                    return;
                }
                else if (!register.photonView.IsMine && !register.IsReady)
                {
                    Debug.Log("Wait for all player to be ready");
                    return;
                }
            }

            // go to new scene
            Debug.Log("2");
            LoadGameplayScene();
        }


        private void LoadGameplayScene()
        {
            if (_playerLobbyRegisters.Count > 1 && !_gameSceneLoaded)
            {
                Debug.Log("Start new Scene here");
                PhotonNetwork.LoadLevel(2);
                _gameSceneLoaded = true;
            }
        }
    }
}

