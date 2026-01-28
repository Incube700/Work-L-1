using System;
using UnityEngine;

public sealed class KeyboardInputReader
{
    public event Action<char> CharTyped;
    public event Action SpacePressed;
    
    public  event Action StatsPressed;
    public  event Action ResetPressed;
    
    public event Action WipePressed;

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpacePressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            StatsPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ResetPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            WipePressed?.Invoke();
        }

        string input = Input.inputString;

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (char.IsControl(c))
            {
                continue;
            }

            CharTyped?.Invoke(c);
        }
    }
}