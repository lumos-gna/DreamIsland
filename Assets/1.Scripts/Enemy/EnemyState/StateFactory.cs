using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태 객체를 캐싱해서 재사용할 수 있도록 관리
public class StateFactory<T>
{
    // 상태 타입을 키로 하여 각 상태 저장
    private Dictionary<System.Type, IState<T>> _state = new();
    
    // 제네릭 제약 조건 : IState 구현 및 기본 생성자 존재
    public Type Get<Type>() where Type : IState<T>, new ()
    {
        System.Type stateType = typeof(Type);

        // 이미 존재하면 저장된거 반환
        if(_state.TryGetValue(stateType, out IState<T> state))
        {
            return (Type)state;
        }

        // 없으면 새로 생성하고 저장
        Type newState = new Type();
        _state[stateType] = newState; 
        return newState;
    }
}
