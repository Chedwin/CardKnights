using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public string stateName;

    public abstract void EnterState(T _owner);
    public abstract void ExitState(T _owner);
    public abstract void UpdateState(T _owner);
}

public class FiniteStateMachine<T>
{
    public State<T> CurrentState { get; private set; }
    public T owner;

    public FiniteStateMachine(T obj)
    {
        owner = obj;
        CurrentState = null;
    }

    public void ChangeState(State<T> _state)
    {
        if (CurrentState != null)
            CurrentState.ExitState(owner);

        CurrentState = _state;
        CurrentState.EnterState(owner);

    }

    public void UpdateState()
    {
        if (CurrentState != null)
            CurrentState.UpdateState(owner);
    }

} // end class FiniteStateMachine<T>

