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
}