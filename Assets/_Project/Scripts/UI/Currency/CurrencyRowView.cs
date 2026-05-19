using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CurrencyRowView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite _goldIcon;
    [SerializeField] private Sprite _diamondIcon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _amountText;

    private void Awake()
    {
        ValidateSerializedReferences();
        ConfigureGraphics();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
        ConfigureGraphics();
    }

    public void SetCurrency(CurrencyType type)
    {
        SetName(GetDisplayName(type));
        SetIcon(GetIcon(type));
    }

    public void SetName(string value)
    {
        if (_nameText != null)
        {
            _nameText.text = value ?? string.Empty;
        }
    }

    public void SetAmount(int value)
    {
        if (_amountText != null)
        {
            _amountText.text = value.ToString("N0");
        }
    }

    private string GetDisplayName(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Gold:
                return "Gold";
            case CurrencyType.Diamond:
                return "Diamonds";
            default:
                return type.ToString();
        }
    }

    private Sprite GetIcon(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Gold:
                return _goldIcon;
            case CurrencyType.Diamond:
                return _diamondIcon;
            default:
                return null;
        }
    }

    private void SetIcon(Sprite icon)
    {
        if (_iconImage == null)
        {
            return;
        }

        _iconImage.sprite = icon;
        _iconImage.enabled = icon != null;
    }

    private void ConfigureGraphics()
    {
        SetRaycastTarget(_iconImage, false);
        SetRaycastTarget(_nameText, false);
        SetRaycastTarget(_amountText, false);
    }

    private void SetRaycastTarget(Graphic graphic, bool isEnabled)
    {
        if (graphic != null)
        {
            graphic.raycastTarget = isEnabled;
        }
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_iconImage, nameof(_iconImage));
        LogMissingReference(_nameText, nameof(_nameText));
        LogMissingReference(_amountText, nameof(_amountText));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(CurrencyRowView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
