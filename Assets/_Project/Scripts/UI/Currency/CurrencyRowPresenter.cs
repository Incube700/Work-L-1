public sealed class CurrencyRowPresenter
{
    private readonly CurrencyRowView _view;
    private readonly WalletService _wallet;
    private readonly CurrencyType _type;

    private IReadOnlyReactiveVariable<int> _amount;

    public CurrencyRowView View => _view;

    public CurrencyRowPresenter(CurrencyRowView view, WalletService wallet, CurrencyType type)
    {
        _view = view;
        _wallet = wallet;
        _type = type;
    }

    public void Initialize()
    {
        _view.SetName(_type.ToString());

        _amount = _wallet.GetReactive(_type);
        _amount.Changed += OnAmountChanged;

        Refresh();
    }

    public void Dispose()
    {
        if (_amount != null)
        {
            _amount.Changed -= OnAmountChanged;
            _amount = null;
        }
    }

    private void OnAmountChanged()
    {
        Refresh();
    }

    private void Refresh()
    {
        _view.SetAmount(_wallet.Get(_type));
    }
}