using UnityEngine;

public sealed class DefendGameplayScreenView : MonoBehaviour
{
    [SerializeField] private DefendHudView _hudView;
    [SerializeField] private PopupLayer _popupLayer;
    [SerializeField] private CurrencyListView _currencyListView;
    

    public CurrencyListView CurrencyListView => _currencyListView;
    public DefendHudView HudView => _hudView;
    public PopupLayer PopupLayer => _popupLayer;
}