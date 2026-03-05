public sealed class DefendGameplayArgs : IInputArgs
{
    public DefendLevelConfig LevelConfig { get; }

    public DefendGameplayArgs(DefendLevelConfig levelConfig)
    {
        LevelConfig = levelConfig;
    }
}
