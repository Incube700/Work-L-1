using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PermanentUpgradesMenuView : MonoBehaviour
{
    [Serializable]
    private sealed class EntryBinding
    {
        public PermanentUpgradeType Type;
        public PermanentUpgradeEntryView View;
    }

    private readonly Dictionary<PermanentUpgradeType, PermanentUpgradeEntryView> _entries =
        new Dictionary<PermanentUpgradeType, PermanentUpgradeEntryView>();

    public event Action CloseClicked;
    public event Action<PermanentUpgradeType> PurchaseRequested;

    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private PermanentUpgradeEntryView _waveHealEntry;
    [SerializeField] private PermanentUpgradeEntryView _openingStrikeEntry;
    [SerializeField] private PermanentUpgradeEntryView _playerExplosionDamageEntry;

    public bool IsVisible => _root != null && _root.activeSelf;

    private void Awake()
    {
        if (_root == null)
        {
            _root = gameObject;
        }

        _entries.Clear();
        _entries[PermanentUpgradeType.WaveHeal] = _waveHealEntry;
        _entries[PermanentUpgradeType.OpeningStrike] = _openingStrikeEntry;
        _entries[PermanentUpgradeType.PlayerExplosionDamage] = _playerExplosionDamageEntry;
    }

    private void OnEnable()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        if (_waveHealEntry != null)
        {
            _waveHealEntry.BuyClicked += OnWaveHealBuyClicked;
        }

        if (_openingStrikeEntry != null)
        {
            _openingStrikeEntry.BuyClicked += OnOpeningStrikeBuyClicked;
        }

        if (_playerExplosionDamageEntry != null)
        {
            _playerExplosionDamageEntry.BuyClicked += OnPlayerExplosionDamageBuyClicked;
        }
    }

    private void OnDisable()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        if (_waveHealEntry != null)
        {
            _waveHealEntry.BuyClicked -= OnWaveHealBuyClicked;
        }

        if (_openingStrikeEntry != null)
        {
            _openingStrikeEntry.BuyClicked -= OnOpeningStrikeBuyClicked;
        }

        if (_playerExplosionDamageEntry != null)
        {
            _playerExplosionDamageEntry.BuyClicked -= OnPlayerExplosionDamageBuyClicked;
        }
    }

    public void Show()
    {
        _root.SetActive(true);
        transform.SetAsLastSibling();
    }

    public void Hide()
    {
        _root.SetActive(false);
    }

    public void SetStatus(string message)
    {
        if (_statusText != null)
        {
            _statusText.text = message ?? string.Empty;
        }
    }

    public void SetEntry(
        PermanentUpgradeType type,
        string title,
        string description,
        int costDiamonds,
        bool purchased,
        bool canAfford)
    {
        if (_entries.TryGetValue(type, out PermanentUpgradeEntryView entryView) == false)
        {
            return;
        }

        string priceText = purchased
            ? "Purchased"
            : $"Cost: {costDiamonds} diamonds";

        string buttonText = purchased ? "Purchased" : "Buy";

        Color priceColor = purchased
            ? new Color(0.65f, 0.95f, 0.72f, 1f)
            : canAfford
                ? new Color(0.96f, 0.95f, 0.9f, 1f)
                : new Color(1f, 0.77f, 0.77f, 1f);

        entryView.SetData(
            title,
            description,
            priceText,
            buttonText,
            purchased == false,
            priceColor);
    }

    private void OnCloseButtonClicked()
    {
        CloseClicked?.Invoke();
    }

    private void OnWaveHealBuyClicked()
    {
        PurchaseRequested?.Invoke(PermanentUpgradeType.WaveHeal);
    }

    private void OnOpeningStrikeBuyClicked()
    {
        PurchaseRequested?.Invoke(PermanentUpgradeType.OpeningStrike);
    }

    private void OnPlayerExplosionDamageBuyClicked()
    {
        PurchaseRequested?.Invoke(PermanentUpgradeType.PlayerExplosionDamage);
    }
}