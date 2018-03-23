namespace FiniteStateMachine
{
    public class FiniteStateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        public T Parent;

        public FiniteStateMachine(T parent)
        {
            Parent = parent;
            CurrentState = null;
        }

        public void Change(State<T> newState)
        {
            if (CurrentState != null)
                CurrentState.Exit(Parent);
            CurrentState = newState;
            CurrentState.Enter(Parent);
        }

        public void Update()
        {
            if (CurrentState != null)
                CurrentState.Update(Parent);
        }

    }

    public abstract class State<T>
    {
        public abstract void Enter(T parent);
        public abstract void Exit(T parent);
        public abstract void Update(T parent);
    }
}
