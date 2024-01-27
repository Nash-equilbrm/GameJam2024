using Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HaloKero.Lobby;


namespace HaloKero.Gameplay
{
    public class GameflowManager : Singleton<GameflowManager>
    {
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
        }

        public void ChangeState(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.MainMenu:
                    {
                        _stateMachine.ChangeState(_menuState); break;
                    }
                case GameFlowState.Lobby:
                    {
                        _stateMachine.ChangeState(_lobbyState); break;
                    }
                case GameFlowState.Gameplay:
                    {
                        _stateMachine.ChangeState(_gameplayState); break;
                    }
                default: break;
            }
        }


        
    }
}

