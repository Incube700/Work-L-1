using TMPro;
using UnityEngine;

public sealed class GameplayTargetView : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        if (_text == null)
        {
            throw new MissingReferenceException($"{nameof(GameplayTargetView)} requires {nameof(TMP_Text)} on the same GameObject.");
        }
    }

    public void SetTarget(string target)
    {
        _text.text = $"Target: {target}";
    }
}