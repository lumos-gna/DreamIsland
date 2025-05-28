using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private IState<T> _currentState;
    private T _obj;

    public StateMachine(T obj)
    {
        _obj = obj;
    }

    public void ChangeState(IState<T> newState)
    {
        _currentState?.Exit(_obj);
        _currentState = newState;
        _currentState?.Enter(_obj);
    }

    public void Update()
    {
        _currentState?.Update(_obj);
    }

    public IState<T> CurrentState => _currentState;
}
