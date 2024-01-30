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
        [SerializeField] private GameObject[] _playerDummyPrefabs;
        public GameObject[] PlayerDummyPrefabs { get => _playerDummyPrefabs; }

        private GameObject _player = null;
        public GameObject Player { get => _player; set => _player = value; }


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

