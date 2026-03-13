using UnityEngine;

public sealed class DefendGameplayScreenView : MonoBehaviour
{
    [SerializeField] private GameplayHudView _hudView;
    [SerializeField] private CurrencyListView _currencyListView;
    [SerializeField] private StatsView _statsView;

    public GameplayHudView HudView => _hudView;
    public CurrencyListView CurrencyListView => _currencyListView;
    public StatsView StatsView => _statsView;
}