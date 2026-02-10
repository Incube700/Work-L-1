using TMPro;
using UnityEngine;

public sealed class GameplayHudView : MonoBehaviour
{
    [SerializeField] private TMP_Text _targetText;
    [SerializeField] private TMP_Text _typedText;
    [SerializeField] private TMP_Text _statusText;

    public void SetTarget(string target) => _targetText.text = $"Target: {target}";
    public void SetTyped(string typed) => _typedText.text = $"Typed: {typed}";
    public void SetStatus(string status) => _statusText.text = status ?? string.Empty;
}