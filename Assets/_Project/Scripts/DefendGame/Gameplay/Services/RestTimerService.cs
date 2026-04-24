using System;

public sealed class RestTimerService
{
    private float _remainingSeconds;
    private bool _isRunning;

    public event Action Changed;

    public float RemainingSeconds => _remainingSeconds;
    public bool IsRunning => _isRunning;

    public void Start(float durationSeconds)
    {
        if (durationSeconds < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(durationSeconds));
        }

        _remainingSeconds = durationSeconds;
        _isRunning = true;

        Changed?.Invoke();
    }

    public void Tick(float deltaTime)
    {
        if (_isRunning == false)
        {
            return;
        }

        if (deltaTime < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(deltaTime));
        }

        float newRemainingSeconds = _remainingSeconds - deltaTime;

        if (newRemainingSeconds <= 0f)
        {
            _remainingSeconds = 0f;
            _isRunning = false;

            Changed?.Invoke();
            return;
        }

        _remainingSeconds = newRemainingSeconds;
        Changed?.Invoke();
    }

    public void Stop()
    {
        if (_isRunning == false && _remainingSeconds <= 0f)
        {
            return;
        }

        _remainingSeconds = 0f;
        _isRunning = false;

        Changed?.Invoke();
    }
}