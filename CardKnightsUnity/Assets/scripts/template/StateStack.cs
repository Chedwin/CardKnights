using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

public class StateStack<T>
{
    public int StateCount
    {
        get
        {
            return _mStateStack.Count;
        }
    }

    protected Stack<IState<T>> _mStateStack;
    public T owner;
    public IState<T> CurrentState { get; private set; }


    public StateStack(T _obj, int _count = 0)
    {
        _mStateStack = new Stack<IState<T>>(_count);
        owner = _obj;
    }

    void PushState(ref IState<T> _state)
    {
        _mStateStack.Push(_state); 
    }

    void PopState()
    {
        _mStateStack.Pop();
    }

    public IState<T> PeekState()
    {
        return _mStateStack.Peek();
    }

    public void AdvanceState(ref IState<T> _state)
    {
        if (CurrentState != null)
            CurrentState.ExitState(owner);

        PushState(ref _state);
        CurrentState = PeekState();
        CurrentState.EnterState(owner);
    }

    public void ReturnState()
    {
        if (StateCount == 1)
            return;

        if (CurrentState != null)
        {
            CurrentState.ExitState(owner);
            PopState();
        }

        CurrentState = PeekState();
        CurrentState.EnterState(owner);
    }

    public void UpdateState()
    {
        if (CurrentState != null)
            CurrentState.UpdateState(owner);
    }

} // end class StateStack
