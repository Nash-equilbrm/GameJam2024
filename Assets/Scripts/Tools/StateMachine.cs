using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tools
{
    public class StateMachine<T>
    {
        private State<T> _currentState;
        public State<T> CurrentState { get => _currentState; set => _currentState = value; }

        private State<T> _previousState;
        public State<T> PreviousState { get => _previousState; set => _previousState = value; }

        public void Initialize(State<T> startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(State<T> newState)
        {
            CurrentState.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            newState.Enter();
        }
    }


    public abstract class  State<T>
    {
        protected T _context;
        public State(T context)
        {
            _context = context;
        }

        public virtual void Enter()
        {
            
        }

        public virtual void HandleInput()
        {

        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {

        }
    }
}
