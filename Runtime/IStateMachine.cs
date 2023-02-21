public interface IStateMachine
{
    public bool TryChangeTo(IState state);
    public T Get<T>() where T : IState;
    public bool TryAdd<T>(T state) where T : IState;
    public bool TryRemove<T>() where T : IState;
}