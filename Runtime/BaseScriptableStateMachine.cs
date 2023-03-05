using UnityEngine;

public abstract class BaseScriptableStateMachine<T> : ScriptableObject, IStateMachine where T : IStateMachine, new()
{
    public T StateMachine { get; } = new();
    
    public bool TryChangeTo(IState state)
    {
        return StateMachine.TryChangeTo(state);
    }

    public T1 Get<T1>() where T1 : IState
    {
        return StateMachine.Get<T1>();
    }

    public bool TryAdd<T1>(T1 state) where T1 : IState
    {
        return StateMachine.TryAdd(state);
    }

    public bool TryRemove<T1>() where T1 : IState
    {
        return StateMachine.TryRemove<T1>();
    }
}