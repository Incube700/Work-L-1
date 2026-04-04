using UnityEngine;

public sealed class MainMenuScreenView : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private PopupLayer _popupLayer;
    [SerializeField] private CurrencyListView _currencyListView;
    [SerializeField] private StatsView _statsView;
    
    public MainMenuView MainMenuView => _mainMenuView;
    public PopupLayer PopupLayer => _popupLayer;
    public CurrencyListView CurrencyListView => _currencyListView;
    public StatsView StatsView => _statsView;
}