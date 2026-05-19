using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MessagePopupView : PopupViewBase
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite _victoryIcon;
    [SerializeField] private Sprite _defeatIcon;
    [SerializeField] private Button _okButton;

    protected override void Awake()
    {
        base.Awake();
        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    private void OnEnable()
    {
        if (_okButton != null)
        {
            _okButton.onClick.AddListener(OnOkClicked);
        }
    }

    private void OnDisable()
    {
        if (_okButton != null)
        {
            _okButton.onClick.RemoveListener(OnOkClicked);
        }
    }

    public void SetTitle(string title)
    {
        if (_titleText != null)
        {
            _titleText.text = title ?? string.Empty;
        }

        UpdateIcon(title);
    }

    public void SetMessage(string message)
    {
        if (_messageText != null)
        {
            _messageText.text = message ?? string.Empty;
        }
    }

    private void UpdateIcon(string title)
    {
        if (_iconImage == null)
        {
            return;
        }

        if (title == "Victory")
        {
            _iconImage.sprite = _victoryIcon;
            _iconImage.enabled = _victoryIcon != null;
            return;
        }

        if (title == "Defeat")
        {
            _iconImage.sprite = _defeatIcon;
            _iconImage.enabled = _defeatIcon != null;
            return;
        }

        _iconImage.sprite = null;
        _iconImage.enabled = false;
    }

    private void OnOkClicked()
    {
        OnCloseButtonClicked();
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_titleText, nameof(_titleText));
        LogMissingReference(_messageText, nameof(_messageText));
        LogMissingReference(_iconImage, nameof(_iconImage));
        LogMissingReference(_victoryIcon, nameof(_victoryIcon));
        LogMissingReference(_defeatIcon, nameof(_defeatIcon));
        LogMissingReference(_okButton, nameof(_okButton));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(MessagePopupView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
