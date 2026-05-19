using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PermanentUpgradesMenuView : MonoBehaviour
{
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
        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    public void Initialize()
    {
        if (_root == null)
        {
            _root = gameObject;
        }

        _entries.Clear();

        AddEntry(PermanentUpgradeType.WaveHeal, _waveHealEntry);
        AddEntry(PermanentUpgradeType.OpeningStrike, _openingStrikeEntry);
        AddEntry(PermanentUpgradeType.PlayerExplosionDamage, _playerExplosionDamageEntry);
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
        bool isInteractable = purchased == false && canAfford;

        entryView.SetData(
            title,
            description,
            priceText,
            buttonText,
            isInteractable);
    }

    private void AddEntry(PermanentUpgradeType type, PermanentUpgradeEntryView entryView)
    {
        if (entryView == null)
        {
            Debug.LogError($"Permanent upgrade entry is not assigned: {type}");
            return;
        }

        _entries[type] = entryView;
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

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_statusText, nameof(_statusText));
        LogMissingReference(_closeButton, nameof(_closeButton));
        LogMissingReference(_waveHealEntry, nameof(_waveHealEntry));
        LogMissingReference(_openingStrikeEntry, nameof(_openingStrikeEntry));
        LogMissingReference(_playerExplosionDamageEntry, nameof(_playerExplosionDamageEntry));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(PermanentUpgradesMenuView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
