using Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HaloKero.Lobby;
using System.Linq;


namespace HaloKero.Gameplay
{
    public class GameflowManager : Singleton<GameflowManager>
    {
        [Header("Lobby Management")]
        public GameObject playerLobbyRegisterPrefab;
        [SerializeField] private GameObject[] _playerDummyPrefabs;
        [SerializeField] private List<PlayerLobbyRegister> _playerLobbyRegisters;

        private int _localPlayerID = -1;
        private bool _gameSceneLoaded = false;
        public GameObject LocalPlayerDummyPrefabs => _playerDummyPrefabs[_localPlayerID];
        public GameObject[] PlayerDummyPrefabs { get => _playerDummyPrefabs; }
        public List<PlayerLobbyRegister> PlayerLobbyRegisters { get => _playerLobbyRegisters; }




        private StateMachine<GameflowManager> _stateMachine = new StateMachine<GameflowManager>();
        private MainMenuState _menuState;
        private LobbyState _lobbyState;
        private GameplayState _gameplayState;

        private void Start()
        {
            _menuState = new MainMenuState(this);
            _lobbyState = new LobbyState(this);
            _gameplayState = new GameplayState(this);

            _stateMachine.Initialize(_menuState);
        }


        private void Update()
        {
            Debug.Log(s);
        }
        string s = "menu";
        public void ChangeState(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.MainMenu:
                    {
                        s = "menu";
                        _stateMachine.ChangeState(_menuState); break;
                    }
                case GameFlowState.Lobby:
                    {
                        s = "Lobby";

                        _stateMachine.ChangeState(_lobbyState); break;
                    }
                case GameFlowState.Gameplay:
                    {
                        s = "Gameplay";
                        _stateMachine.ChangeState(_gameplayState); break;
                    }
                default: break;
            }
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


        internal void CheckStartGameRequirements(object data = null)
        {
            if (_playerLobbyRegisters.Count <= 1)
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
        }


        private void StartGameplay()
        {
            if (_playerLobbyRegisters.Count > 1 && !_gameSceneLoaded)
            {
                //Debug.Log("Start new gameplay here");
                //PhotonNetwork.Instantiate(_playerDummyPrefabs[PhotonNetwork.LocalPlayer.ActorNumber].name, Vector3.zero, Quaternion.identity);
                UIManager.Instance.HideAllScreens();
                GameflowManager.Instance.ChangeState(GameFlowState.Gameplay);
            }
        }

    }
}

