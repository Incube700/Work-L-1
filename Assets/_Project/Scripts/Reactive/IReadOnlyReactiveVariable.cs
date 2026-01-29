using System;

public interface IReadOnlyReactiveVariable<T>
{
    event Action Changed;
    T Value { get; }
}