using UnityEngine;
using UnityEngine.UI;

public sealed class CurrencyListView : MonoBehaviour
{
    [SerializeField] private Transform _content;

    [Header("Optional Generated Skin")]
    [SerializeField] private GameObject _optionalSkinRoot;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _frameImage;

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
