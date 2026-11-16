using UnityEngine;
//gestiona el estado activo y permite cambiar de uno a otro:
public class StateMachine
{
    public IState currentState { get; private set; }

    public void Initialize(IState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }
}
