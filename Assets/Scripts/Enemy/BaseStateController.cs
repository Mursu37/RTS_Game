
using UnityEngine;

namespace Enemy
{
    public abstract class BaseStateController
    {
        private State _currentState;

        public abstract void Initialize();
        
        public virtual void ChangeState(State newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }

        public void FixedUpdate()
        {
            _currentState.OnFixedUpdate();
        }

        public void Update()
        {
            _currentState.OnUpdate();
        }
    }
}