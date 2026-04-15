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
        bool isInteractable,
        Color priceColor)
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
            _priceText.color = priceColor;
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
}