using TMPro;
using UnityEngine;

public sealed class CurrencyRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _amountText;

    public void SetName(string value)
    {
        _nameText.text = value ?? string.Empty;
    }

    public void SetAmount(int value)
    {
        _amountText.text = value.ToString();
    }
}