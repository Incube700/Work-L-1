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
}