using System;
using Assets._Project.Scripts.UI.Common;

public sealed class PermanentUpgradesMenuPresenter : IPresenter
{
    private readonly MainMenuView _mainMenuView;
    private readonly PermanentUpgradesService _permanentUpgradesService;
    private readonly WalletService _walletService;
    private readonly SaveService _saveService;
    private readonly EconomyConfig _economy;

    private PermanentUpgradesMenuView _shopView;
    private IReadOnlyReactiveVariable<int> _diamonds;
    private bool _isInitialized;

    public PermanentUpgradesMenuPresenter(
        MainMenuView mainMenuView,
        PermanentUpgradesService permanentUpgradesService,
        WalletService walletService,
        ConfigService configService,
        SaveService saveService)
    {
        _mainMenuView = mainMenuView ?? throw new ArgumentNullException(nameof(mainMenuView));
        _permanentUpgradesService = permanentUpgradesService ??
            throw new ArgumentNullException(nameof(permanentUpgradesService));
        _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));

        if (configService == null)
        {
            throw new ArgumentNullException(nameof(configService));
        }

        _economy = configService.Load<EconomyConfig>();
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

        _diamonds = _walletService.GetReactive(CurrencyType.Diamond);
        _diamonds.Changed += OnDiamondsChanged;

        _mainMenuView.UpgradesClicked += OnUpgradesClicked;
        _shopView.CloseClicked += OnCloseClicked;
        _shopView.PurchaseRequested += OnPurchaseRequested;
        _permanentUpgradesService.Changed += OnPermanentUpgradesChanged;

        _shopView.Hide();
        RefreshEntries();

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

        _shopView.SetStatus(string.Empty);
        RefreshEntries();
        _shopView.Show();
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
        int cost = _permanentUpgradesService.GetCost(type);
        bool purchased = _permanentUpgradesService.IsPurchased(type);
        bool canAfford = _walletService.Get(CurrencyType.Diamond) >= cost;

        _shopView.SetEntry(
            type,
            GetTitle(type),
            GetDescription(type),
            cost,
            purchased,
            canAfford);
    }

    private string GetTitle(PermanentUpgradeType type)
    {
        switch (type)
        {
            case PermanentUpgradeType.WaveHeal:
                return "Emergency Repairs";

            case PermanentUpgradeType.OpeningStrike:
                return "Opening Strike";

            case PermanentUpgradeType.PlayerExplosionDamage:
                return "Arcane Overload";

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private string GetDescription(PermanentUpgradeType type)
    {
        switch (type)
        {
            case PermanentUpgradeType.WaveHeal:
                return $"Heal the tower by {_economy.WaveHealPercent:0.#}% at the start of every wave.";

            case PermanentUpgradeType.OpeningStrike:
                return
                    $"Deal {_economy.OpeningStrikeDamagePercent:0.#}% damage to the first {_economy.OpeningStrikeTargetsCount} enemies of each wave.";

            case PermanentUpgradeType.PlayerExplosionDamage:
                return $"Increase player click explosion damage by {_economy.PlayerExplosionDamagePercent:0.#}% for the whole run.";

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
