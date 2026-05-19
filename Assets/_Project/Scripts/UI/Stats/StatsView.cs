using TMPro;
using UnityEngine;

public sealed class StatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _winsText;
    [SerializeField] private TMP_Text _lossesText;

    [SerializeField] private string _winsPrefix = "Wins: ";
    [SerializeField] private string _lossesPrefix = "Losses: ";

    private void Awake()
    {
        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    public void SetWins(int value)
    {
        _winsText.text = _winsPrefix + value;
    }

    public void SetLosses(int value)
    {
        _lossesText.text = _lossesPrefix + value;
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_winsText, nameof(_winsText));
        LogMissingReference(_lossesText, nameof(_lossesText));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(StatsView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
