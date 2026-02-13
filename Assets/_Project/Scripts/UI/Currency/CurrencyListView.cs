using UnityEngine;

public sealed class CurrencyListView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    
    public Transform Content => _content;

    private void Awake()
    {
        if (_content == null)
        {
            throw new MissingReferenceException($"{nameof(CurrencyListView)} is not set");
        }
    }
}