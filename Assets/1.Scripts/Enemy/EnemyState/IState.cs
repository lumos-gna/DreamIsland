using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void Enter(T obj);
    void Update(T obj);
    void Exit(T obj);
}
