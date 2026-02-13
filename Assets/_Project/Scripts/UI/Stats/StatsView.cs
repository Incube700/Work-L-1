using TMPro;
using UnityEngine;

public sealed class StatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private TMP_Text _lossesText;

    [SerializeField] private string _winsPrefix = "Wins: ";
    [SerializeField] private string _lossesPrefix = "Losses: ";

    public void SetWins(int value)
    {
        _winsText.text = _winsPrefix + value;
    }

    public void SetLosses(int value)
    {
        _lossesText.text = _lossesPrefix + value;
    }
}