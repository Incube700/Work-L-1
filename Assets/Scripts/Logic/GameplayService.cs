using UnityEngine;

public class GameplayService
{
    private readonly string _targetSequence;
    private int _currentIndex;
    private bool _isGameOver;

    public bool IsGameOver => _isGameOver;
    public bool IsWin { get; private set; }

    public GameplayService(string availableChars, int length)
    {
        if (string.IsNullOrEmpty(availableChars))
        {
            throw new System.ArgumentException("Available chars is empty.", nameof(availableChars));
        }

        if (length <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(length));
        }

        _targetSequence = "";

        for (int i = 0; i < length; i++)
        {
            char c = availableChars[Random.Range(0, availableChars.Length)];
            _targetSequence += char.ToUpperInvariant(c);
        }

        Debug.Log($"ЗАГАДАНО: {_targetSequence}");
        Debug.Log("Вводи символы. Ошибся — проиграл. В конце жми ПРОБЕЛ.");
    }

    public void HandleInput(char inputChar)
    {
        if (_isGameOver)
        {
            return;
        }

        if (char.IsControl(inputChar))
        {
            return;
        }

        char typed = char.ToUpperInvariant(inputChar);
        char expected = _targetSequence[_currentIndex];

        if (typed != expected)
        {
            Lose(typed, expected);
            return;
        }

        _currentIndex++;

        if (_currentIndex >= _targetSequence.Length)
        {
            Win();
        }
    }

    private void Win()
    {
        _isGameOver = true;
        IsWin = true;
        Debug.Log("ПОБЕДА! Нажми Пробел, чтобы вернуться в меню.");
    }

    private void Lose(char typed, char expected)
    {
        _isGameOver = true;
        IsWin = false;
        Debug.Log($"ПОРАЖЕНИЕ! Ожидалось '{expected}', а ты ввёл '{typed}'. Нажми Пробел, чтобы попробовать снова.");
    }
}