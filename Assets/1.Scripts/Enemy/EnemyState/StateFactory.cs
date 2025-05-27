using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태 객체를 캐싱해서 재사용할 수 있도록 관리하는 클래스
public class StateFactory<T>
{
    // 상태 타입을 키로 하여 각 상태 저장
    private readonly Dictionary<System.Type, IState<T>> _state = new();
    public State Get<State>() where State : IState<T>, new ()
    {
        System.Type stateType = typeof(State);

        // 이미 존재하면 저장된 인스턴스 반환
        if(_state.TryGetValue(stateType, out IState<T> state))
        {
            return (State)state;
        }

        // 없으면 새로 생성하고 저장
        State newState = new State();
        _state[stateType] = newState; 
        return newState;
    }
}
