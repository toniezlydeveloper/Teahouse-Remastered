using System;
using System.Collections.Generic;
using System.Linq;

namespace Internal.Flow.States
{
    // ReSharper disable once VirtualMemberCallInConstructor
    public abstract class AState
    {
        public List<StateTransition> Transitions { get; } = new();

        protected AState() => AddConditions();
        
        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual Type OnUpdate() => null;

        protected virtual void AddConditions()
        {
        }

        protected void AddCondition<TState>(Func<bool> condition) where TState : AState
        {
            if (!TryGetExisting<TState>(out StateTransition transition))
            {
                AddTransition(transition);
            }

            AddCondition(transition, condition);
        }

        private bool TryGetExisting<TState>(out StateTransition transition) where TState : AState
        {
            transition = Transitions.FirstOrDefault(transition => transition.TargetType == typeof(TState));

            if (transition != null)
            {
                return true;
            }
            
            transition = new StateTransition
            {
                Conditions = new List<Func<bool>>(),
                TargetType = typeof(TState)
            };

            return false;
        }
        
        private void AddTransition(StateTransition transition) => Transitions.Add(transition);

        private void AddCondition(StateTransition transition, Func<bool> condition) =>
            transition.Conditions.Add(condition);
    }
}