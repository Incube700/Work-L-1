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
        LogMissingReference(_mainMenuView, nameof(_mainMenuView));
        LogMissingReference(_popupLayer, nameof(_popupLayer));
        LogMissingReference(_currencyListView, nameof(_currencyListView));
        LogMissingReference(_statsView, nameof(_statsView));
    }

    private void LogMissingReference(UnityEngine.Object reference, string fieldName)
    {
        if (reference == null)
        {
            Debug.LogError($"{name}: {nameof(MainMenuScreenView)} missing serialized reference '{fieldName}'.", this);
        }
    }
}
