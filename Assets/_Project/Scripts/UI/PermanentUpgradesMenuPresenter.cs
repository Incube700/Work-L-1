using System;
using Assets._Project.Scripts.UI.Common;

public sealed class PermanentUpgradesMenuPresenter : IPresenter
{
    private readonly MainMenuView _mainMenuView;
    private readonly PermanentUpgradesService _permanentUpgradesService;
    private readonly WalletService _walletService;
    private readonly SaveService _saveService;

    private PermanentUpgradesMenuView _shopView;
    private IReadOnlyReactiveVariable<int> _diamonds;
    private bool _isInitialized;

    public PermanentUpgradesMenuPresenter(
        MainMenuView mainMenuView,
        PermanentUpgradesService permanentUpgradesService,
        WalletService walletService,
        SaveService saveService)
    {
        _mainMenuView = mainMenuView ?? throw new ArgumentNullException(nameof(mainMenuView));
        _permanentUpgradesService = permanentUpgradesService ??
            throw new ArgumentNullException(nameof(permanentUpgradesService));
        _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _shopView = _mainMenuView.UpgradesMenuView;

        if (_shopView == null)
        {
            throw new InvalidOperationException("PermanentUpgradesMenuView is not available.");
        }

        _shopView.Initialize();
        _shopView.Hide();

        _diamonds = _walletService.GetReactive(CurrencyType.Diamond);
        _diamonds.Changed += OnDiamondsChanged;

        _mainMenuView.UpgradesClicked += OnUpgradesClicked;
        _shopView.CloseClicked += OnCloseClicked;
        _shopView.PurchaseRequested += OnPurchaseRequested;
        _permanentUpgradesService.Changed += OnPermanentUpgradesChanged;

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _permanentUpgradesService.Changed -= OnPermanentUpgradesChanged;

        if (_shopView != null)
        {
            _shopView.PurchaseRequested -= OnPurchaseRequested;
            _shopView.CloseClicked -= OnCloseClicked;
        }

        _mainMenuView.UpgradesClicked -= OnUpgradesClicked;

        if (_diamonds != null)
        {
            _diamonds.Changed -= OnDiamondsChanged;
            _diamonds = null;
        }

        _shopView = null;
        _isInitialized = false;
    }

    private void OnUpgradesClicked()
    {
        if (_shopView.IsVisible)
        {
            _shopView.Hide();
            return;
        }

        _shopView.Show();
        _shopView.SetStatus(string.Empty);
        RefreshEntries();
    }

    private void OnCloseClicked()
    {
        _shopView.Hide();
    }

    private void OnPurchaseRequested(PermanentUpgradeType type)
    {
        if (_permanentUpgradesService.TryPurchase(type, out string failureReason))
        {
            _saveService.SaveAll();
            _shopView.SetStatus("Ability purchased.");
        }
        else
        {
            _shopView.SetStatus(failureReason);
        }

        RefreshEntries();
    }

    private void OnPermanentUpgradesChanged()
    {
        RefreshEntries();
    }

    private void OnDiamondsChanged()
    {
        RefreshEntries();
    }

    private void RefreshEntries()
    {
        RefreshEntry(PermanentUpgradeType.WaveHeal);
        RefreshEntry(PermanentUpgradeType.OpeningStrike);
        RefreshEntry(PermanentUpgradeType.PlayerExplosionDamage);
    }

    private void RefreshEntry(PermanentUpgradeType type)
    {
        PermanentUpgradeConfigBase config = _permanentUpgradesService.GetConfig(type);

        bool purchased = _permanentUpgradesService.IsPurchased(type);
        bool canAfford = _walletService.Get(CurrencyType.Diamond) >= config.CostDiamonds;

        _shopView.SetEntry(
            type,
            config.Title,
            config.Description,
            config.CostDiamonds,
            purchased,
            canAfford);
    }
}