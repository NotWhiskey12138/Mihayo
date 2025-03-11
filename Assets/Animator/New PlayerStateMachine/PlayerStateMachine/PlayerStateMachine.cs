using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class PlayerStateMachine
    {
        public PlayerState currentState { get; private set; }

        public void InitializeState(PlayerState startState)
        {
            currentState = startState;
            currentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}

