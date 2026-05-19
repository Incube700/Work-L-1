using UnityEngine;

public sealed class CurrencyListView : MonoBehaviour
{
    [SerializeField] private Transform _content;

    public Transform Content => _content;

    private void Awake()
    {
        if (_content == null)
        {
            Debug.LogError($"{name}: {nameof(CurrencyListView)} missing serialized reference '{nameof(_content)}'.", this);
            throw new MissingReferenceException($"{name}: {nameof(CurrencyListView)} missing serialized reference '{nameof(_content)}'.");
        }
    }
}
