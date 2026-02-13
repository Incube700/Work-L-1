using System.Collections.Generic;

public sealed class CurrencyListPresenter
{
    private readonly CurrencyListView _view;
    private readonly ViewsFactory _views;
    private readonly WalletService _wallet;

    private readonly List<CurrencyRowPresenter> _rows = new List<CurrencyRowPresenter>();

    public CurrencyListPresenter(CurrencyListView view, ViewsFactory views, WalletService wallet)
    {
        _view = view;
        _views = views;
        _wallet = wallet;
    }

    public void Initialize()
    {
        CurrencyType[] types = _wallet.GetAvailableCurrencies();

        for (int i = 0; i < types.Length; i++)
        {
            CurrencyType type = types[i];

            CurrencyRowView rowView = _views.Create<CurrencyRowView>(ViewIDs.CurrencyRow, _view.Content);
            CurrencyRowPresenter rowPresenter = new CurrencyRowPresenter(rowView, _wallet, type);

            rowPresenter.Initialize();
            _rows.Add(rowPresenter);
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < _rows.Count; i++)
        {
            CurrencyRowPresenter presenter = _rows[i];
            presenter.Dispose();
            _views.Release(presenter.View);
        }

        _rows.Clear();
    }
}