public sealed class GameplayArgs : IInputArgs
{
    public GameMode Mode { get; }

    public GameplayArgs(GameMode mode)
    {
        Mode = mode;
    }
}