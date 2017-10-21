using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

public class FiniteStateMachine<T>
{
    public IState<T> CurrentState { get; private set; }
    public T owner;

    protected List<IState<T>> stateList;// = new List<IState<T>>();
      
    public FiniteStateMachine(T obj)
    {
        owner = obj;
        CurrentState = null;
        stateList = new List<IState<T>>();
    }

    public void AddState(ref IState<T> _state)
    {
        stateList.Add(_state);    
    }

    public int GetStateCount()
    {
        return stateList.Count;
    }

    public void ChangeState(IState<T> _state)
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

