using TMPro;
using UnityEngine;

public sealed class GameplayStatusView : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        if (_text == null)
        {
            throw new MissingReferenceException($"{nameof(GameplayStatusView)} requires {nameof(TMP_Text)} on the same GameObject.");
        }
    }

    public void SetStatus(string status)
    {
        _text.text = status ?? string.Empty;
    }
}