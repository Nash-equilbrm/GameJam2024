using Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace HaloKero.Gameplay
{
    public class GameflowManager : Singleton<GameflowManager>
    {
        [Header("Lobby Management")]
        public GameObject playerLobbyRegisterPrefab;
        [SerializeField] private GameObject[] _playerDummyPrefabs;
        //[SerializeField] private List<PlayerLobbyRegister> _playerLobbyRegisters;

        private int _localPlayerID = -1;
        private bool _gameSceneLoaded = false;
        public GameObject LocalPlayerDummyPrefabs => _playerDummyPrefabs[_localPlayerID];
        public GameObject[] PlayerDummyPrefabs { get => _playerDummyPrefabs; }
        //public List<PlayerLobbyRegister> PlayerLobbyRegisters { get => _playerLobbyRegisters; }




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

        string s = "MainMenu";
        private void Update()
        {
            Debug.Log(s);
            _stateMachine.CurrentState.LogicUpdate();
        }


        public void ChangeState(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.MainMenu:
                    {
                        s = "MainMenu";
                        _stateMachine.ChangeState(_menuState); 
                        break;
                    }
                case GameFlowState.Lobby:
                    {
                        s = "Lobby";
                        _stateMachine.ChangeState(_lobbyState);
                        break;
                    }
                case GameFlowState.Gameplay:
                    {
                        s = "Gameplay";
                        _stateMachine.ChangeState(_gameplayState); 
                        break;
                    }
                default: break;
            }
        }







    }
}

