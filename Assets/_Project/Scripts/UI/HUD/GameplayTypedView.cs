using TMPro;
using UnityEngine;

public sealed class GameplayTypedView : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        if (_text == null)
        {
            throw new MissingReferenceException($"{nameof(GameplayTypedView)} requires {nameof(TMP_Text)} on the same GameObject.");
        }
    }

    public void SetTyped(string typed)
    {
        _text.text = $"Typed: {typed}";
    }
}