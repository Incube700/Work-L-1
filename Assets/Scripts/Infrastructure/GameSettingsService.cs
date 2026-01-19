public class GameSettingsService
{
    public GameConfig SelectedConfig { get; private set; }
    public int SequenceLength { get; }

    public GameSettingsService(int sequenceLength)
    {
        if (sequenceLength <= 0)
        {
            throw new System.ArgumentOutOfRangeException(nameof(sequenceLength));
        }

        SequenceLength = sequenceLength;
    }

    public void SetConfig(GameConfig config)
    {
        SelectedConfig = config;
    }
}