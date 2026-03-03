using System;

public sealed class SimpleEvent : IReadOnlySimpleEvent
{
    public event Action Invoked;

    public void Invoke()
    {
        Invoked?.Invoke();
    }
}

public sealed class SimpleEvent<T> : IReadOnlySimpleEvent<T>
{
    public event Action<T> Invoked;

    public void Invoke(T value)
    {
        Invoked?.Invoke(value);
    }
}
