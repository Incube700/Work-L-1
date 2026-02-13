using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuView : MonoBehaviour
{
    // View только сообщает “кнопку нажали”.
    // Логику делаем в презентере.
    public event Action NumbersClicked;
    public event Action LettersClicked;
    public event Action ResetClicked;

    
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TMP_Text _resetButtonText;

    [SerializeField] private Button _numbersButton;
    [SerializeField] private Button _lettersButton;
    [SerializeField] private Button _resetButton;

    private void OnEnable()
    {
        _numbersButton.onClick.AddListener(OnNumbersClicked);
        _lettersButton.onClick.AddListener(OnLettersClicked);
        _resetButton.onClick.AddListener(OnResetClicked);
    }

    private void OnDisable()
    {
        _numbersButton.onClick.RemoveListener(OnNumbersClicked);
        _lettersButton.onClick.RemoveListener(OnLettersClicked);
        _resetButton.onClick.RemoveListener(OnResetClicked);
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
        _statusText.text = message ?? string.Empty;
    }

    private void OnNumbersClicked() => NumbersClicked?.Invoke();
    private void OnLettersClicked() => LettersClicked?.Invoke();
    private void OnResetClicked() => ResetClicked?.Invoke();
}