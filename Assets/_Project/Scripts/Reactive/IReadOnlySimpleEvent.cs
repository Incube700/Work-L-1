using System;

public interface IReadOnlySimpleEvent
{
    event Action Invoked;
}

public interface IReadOnlySimpleEvent<T>
{
    event Action<T> Invoked;
}
