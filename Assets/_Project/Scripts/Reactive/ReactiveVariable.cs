using System;
using System.Collections.Generic;

public sealed class ReactiveVariable<T> : IReadOnlyReactiveVariable<T>
{
    public event Action Changed;
    public event Action<T, T> ValueChanged;

    private readonly EqualityComparer<T> _comparer;
    private T _value;

    public ReactiveVariable(T startValue)
    {
        _value = startValue;
        _comparer = EqualityComparer<T>.Default;
    }

    public T Value
    {
        get => _value;
        set
        {
            T oldValue = _value;

            if (_comparer.Equals(oldValue, value))
                return;

            _value = value;

            ValueChanged?.Invoke(oldValue, _value);
            Changed?.Invoke();
        }
    }
}