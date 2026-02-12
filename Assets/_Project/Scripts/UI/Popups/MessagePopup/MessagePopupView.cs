using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MessagePopupView : PopupViewBase
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _messageText;
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
    }

    public void SetMessage(string message)
    {
        if (_messageText != null)
        {
            _messageText.text = message ?? string.Empty;
        }
    }

    private void OnOkClicked()
    {
        OnCloseButtonClicked();
    }
}