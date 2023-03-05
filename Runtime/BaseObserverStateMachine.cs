using System;
using System.Collections.Generic;

public abstract class BaseObserverStateMachine : IStateMachine
{
    private readonly Dictionary<Type, IState> _stateByType = new();
    private readonly Dictionary<(Type state, Type trigger), object> _triggerEventsByTypes = new();

    public IState Current { get; private set; }
    public IState Previous { get; private set; }

    public void Update()
    {
        Current?.OnUpdate();
    }
    
    public bool TryChangeTo(IState state)
    {
        var stateType = state.GetType();
        
        if (Current == state) return false;
        if (!_stateByType.ContainsKey(stateType)) return false;
        
        Current?.OnExit();
        Previous = Current;
        Current = state;
        Current.OnEnter();

        return true;
    }
    
    public T Get<T>() where T : IState
    {
        var stateType = typeof(T);
        var state = Get(stateType);
        if (state == default) return default;
        return (T) _stateByType[stateType];
    }
    
    public IState Get(Type stateType)
    {
        return !_stateByType.ContainsKey(stateType) ? default : _stateByType[stateType];
    }

    public bool TryAdd<T>(T state) where T : IState
    {
        var stateType = state.GetType();
        var isAdded = _stateByType.TryAdd(stateType, state);
        if (isAdded && _stateByType.Count == 1)
            TryChangeTo(state);

        return isAdded;
    }

    public bool TryRemove<T>() where T : IState
    {
        var stateType = typeof(T);
        return _stateByType.Remove(stateType);
    }

    public void SetTrigger<T>(T trigger) where T : ITrigger
    {
        var stateType = Current.GetType();
        var triggerType = typeof(T);

        var triggerEventFilter = (stateType, triggerType);
        if (!_triggerEventsByTypes.ContainsKey(triggerEventFilter)) return;

        var action = _triggerEventsByTypes[triggerEventFilter] as Action<T>;
        action?.Invoke(trigger);
    }

    public void AddTriggerListener<T>(IState state, Action<T> trigger) where T : ITrigger
    {
        var stateType = state.GetType();
        var triggerType = typeof(T);
        
        var triggerEventFilter = (stateType, triggerType);
        if (_triggerEventsByTypes.ContainsKey(triggerEventFilter)) return;
        _triggerEventsByTypes.Add(triggerEventFilter, trigger);
    }
    
    public void RemoveTriggerListener<T>(IState state) where T : ITrigger
    {
        var stateType = state.GetType();
        var triggerType = typeof(T);
        
        var triggerEventFilter = (stateType, triggerType);
        _triggerEventsByTypes.Remove(triggerEventFilter);
    }
}