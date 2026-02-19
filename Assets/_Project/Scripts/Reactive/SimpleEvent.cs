using System;

public sealed class SimpleEvent
{
    public event Action Invoked;

    public void Invoke()
    {
        Invoked?.Invoke();
    }
}

public sealed class SimpleEvent<T>
{
    public event Action<T> Invoked;

    public void Invoke(T value)
    {
        Invoked?.Invoke(value);
    }
}