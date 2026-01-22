using System.Text;
using UnityEngine;

public sealed class SequenceGenerator
{
    public string Generate(string availableChars, int length)
    {
        if (string.IsNullOrEmpty(availableChars))
        {
            throw new System.ArgumentException("Available chars is empty.", nameof(availableChars));
        }

        if (length <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(length));
        }

        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            char c = availableChars[Random.Range(0, availableChars.Length)];
            sb.Append(char.ToUpperInvariant(c));
        }

        return sb.ToString();
    }
}