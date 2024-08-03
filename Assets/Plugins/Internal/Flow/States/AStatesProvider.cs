using System;
using System.Collections.Generic;
using UnityEngine;

namespace Internal.Flow.States
{
    public abstract class AStatesProvider : MonoBehaviour
    {
        public Dictionary<Type, AState> StatesByType { get; } = new();
        public Type InitialStateType { get; private set; }

        protected void AddState<TState>(TState state) where TState : AState => StatesByType.Add(typeof(TState), state);

        protected void AddInitialState<TState>(TState state) where TState : AState
        {
            StatesByType.Add(typeof(TState), state);
            InitialStateType = typeof(TState);
        }
    }
}