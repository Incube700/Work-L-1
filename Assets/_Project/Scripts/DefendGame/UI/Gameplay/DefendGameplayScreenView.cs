using UnityEngine;

public sealed class DefendGameplayScreenView : MonoBehaviour
{
    [SerializeField] private DefendHudView _hudView;
    [SerializeField] private PlacementPanelView _placementPanelView;
    [SerializeField] private PopupLayer _popupLayer;
    [SerializeField] private CurrencyListView _currencyListView;

    public DefendHudView HudView => _hudView;
    public PlacementPanelView PlacementPanelView => _placementPanelView;
    public PopupLayer PopupLayer => _popupLayer;
    public CurrencyListView CurrencyListView => _currencyListView;

    private void Awake()
    {
        ValidateSerializedReferences();
    }

    private void OnValidate()
    {
        ValidateSerializedReferences();
    }

    private void ValidateSerializedReferences()
    {
        LogMissingReference(_hudView, nameof(_hudView));
        LogMissingReference(_placementPanelView, nameof(_placementPanelView));
        LogMissingReference(_popupLayer, nameof(_popupLayer));
        LogMissingReference(_currencyListView, nameof(_currencyListView));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(DefendGameplayScreenView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
