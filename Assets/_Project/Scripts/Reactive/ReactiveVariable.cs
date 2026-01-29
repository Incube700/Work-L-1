using System;

public sealed class ReactiveVariable<T> : IReadOnlyReactiveVariable<T>
{
    public T Value
    {
        get => _value;
        set
        {
            if (Equals(_value, value))
            {
                return;
            }

            _value = value;
            Changed?.Invoke();
        }
    }

    public event Action Changed;

    private T _value;

    public ReactiveVariable(T startValue)
    {
        _value = startValue;
    }
}