using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PermanentUpgradeEntryView : MonoBehaviour
{
    public event Action BuyClicked;

    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private Button _buyButton;

    private void Awake()
    {
        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    private void OnEnable()
    {
        if (_buyButton != null)
        {
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    private void OnDisable()
    {
        if (_buyButton != null)
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }
    }

    public void SetData(
        string title,
        string description,
        string priceText,
        string buttonText,
        bool isInteractable)
    {
        if (_titleText != null)
        {
            _titleText.text = title ?? string.Empty;
        }

        if (_descriptionText != null)
        {
            _descriptionText.text = description ?? string.Empty;
        }

        if (_priceText != null)
        {
            _priceText.text = priceText ?? string.Empty;
        }

        if (_buttonText != null)
        {
            _buttonText.text = buttonText ?? string.Empty;
        }

        if (_buyButton != null)
        {
            _buyButton.interactable = isInteractable;
        }
    }

    private void OnBuyButtonClicked()
    {
        BuyClicked?.Invoke();
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_titleText, nameof(_titleText));
        LogMissingReference(_descriptionText, nameof(_descriptionText));
        LogMissingReference(_priceText, nameof(_priceText));
        LogMissingReference(_buttonText, nameof(_buttonText));
        LogMissingReference(_buyButton, nameof(_buyButton));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(PermanentUpgradeEntryView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
