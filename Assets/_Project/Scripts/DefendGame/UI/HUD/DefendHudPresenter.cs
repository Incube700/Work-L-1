public sealed class DefendHudPresenter
{
    private readonly DefendHudView _view;
    private readonly WalletService _wallet;
    private readonly DefendGameController _controller;

    public DefendHudPresenter(
        DefendHudView view,
        WalletService wallet,
        DefendGameController controller)
    {
        _view = view;
        _wallet = wallet;
        _controller = controller;
    }

    public void Refresh()
    {
        _view.SetGold(_wallet.Get(CurrencyType.Gold));
        _view.SetWave(_controller.CurrentWaveNumber, _controller.WavesCount);
        _view.SetPhase(_controller.Phase.ToString());
        _view.SetBuildingHealth(_controller.BuildingCurrentHealth, _controller.BuildingMaxHealth);
    }
}
