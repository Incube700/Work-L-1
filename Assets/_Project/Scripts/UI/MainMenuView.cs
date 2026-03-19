using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuView : MonoBehaviour
{
    public event Action PlayClicked;
    public event Action ResetClicked;

    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _resetButtonText;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _resetButton;

    private void OnEnable()
    {
        if (_playButton != null)
        {
            _playButton.onClick.AddListener(OnPlayClicked);
        }

        if (_resetButton != null)
        {
            _resetButton.onClick.AddListener(OnResetClicked);
        }
    }

    private void OnDisable()
    {
        if (_playButton != null)
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
        }

        if (_resetButton != null)
        {
            _resetButton.onClick.RemoveListener(OnResetClicked);
        }
    }

    public void SetResetCost(int cost)
    {
        if (_resetButtonText != null)
        {
            _resetButtonText.text = $"Reset ({cost} gold)";
        }
    }

    public void SetStatus(string message)
    {
        if (_statusText != null)
        {
            _statusText.text = message ?? string.Empty;
        }
    }

    private void OnPlayClicked()
    {
        PlayClicked?.Invoke();
    }

    private void OnResetClicked()
    {
        ResetClicked?.Invoke();
    }
}