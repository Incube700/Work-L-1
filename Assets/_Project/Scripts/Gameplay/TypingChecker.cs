using System;
using UnityEngine;

public sealed class TypingChecker
{
    public event Action Won;
    public event Action Lost;

    private readonly string _target;
    private int _index;

    public TypingChecker(string target)
    {
        this._target = target;
        _index = 0;
    }

    public void HandleChar(char c)
    {
        char typed = char.ToUpperInvariant(c);
        char expected = _target[_index];

        if (typed != expected)
        {
            Lost?.Invoke();
            return;
        }

        _index++;

        if (_index >= _target.Length)
        {
            Won?.Invoke();
        }
    }
}