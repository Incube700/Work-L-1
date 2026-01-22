using System;
using UnityEngine;

public sealed class TypingChecker
{
    public event Action Won;
    public event Action Lost;

    private readonly string target;
    private int index;

    public TypingChecker(string target)
    {
        this.target = target;
        index = 0;
    }

    public void HandleChar(char c)
    {
        char typed = char.ToUpperInvariant(c);
        char expected = target[index];

        if (typed != expected)
        {
            Lost?.Invoke();
            return;
        }

        index++;

        if (index >= target.Length)
        {
            Won?.Invoke();
        }
    }
}