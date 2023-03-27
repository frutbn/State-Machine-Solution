using UnityEngine;

public abstract class BaseMonoStateMachine<T> : MonoBehaviour, IStateMachine where T : class, IStateMachine
{
    public T Instance => this as T;

    public bool TryChangeTo(IState state)
    {
        return Instance.TryChangeTo(state);
    }

    public S Get<S>() where S : IState
    {
        return Instance.Get<S>();
    }

    public bool TryAdd<S>(S state) where S : IState
    {
        return Instance.TryAdd(state);
    }

    public bool TryRemove<S>() where S : IState
    {
        return Instance.TryRemove<S>();
    }
}