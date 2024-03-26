using FrameWork.Utils;

namespace FrameWork.FSM
{
    public abstract class StateMachine
    {
        protected IState currentState { get; private set; }
        
        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            
            currentState = newState;

            currentState.Enter();
        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }

        public void LogicUpdate()
        {
            currentState?.LogicUpdate();
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }
    }
}