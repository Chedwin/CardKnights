using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

public class FiniteStateMachine<T>
{
    public int StateCount
    {
        get
        {
            return _mStateList.Count;
        }
    }

    public IState<T> CurrentState { get; private set; }
    public T owner;

    protected List<IState<T>> _mStateList;// = new List<IState<T>>();
      
    public FiniteStateMachine(T obj)
    {
        owner = obj;
        CurrentState = null;
        _mStateList = new List<IState<T>>();
    }

    public void AddState(ref IState<T> _state)
    {
        _mStateList.Add(_state);    
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

