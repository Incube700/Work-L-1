using System.Collections.Generic;
using Assets._Project.Scripts.UI.Common;

public sealed class CurrencyListPresenter : IPresenter
{
    private readonly CurrencyListView _view;
    private readonly ViewsFactory _views;
    private readonly WalletService _wallet;

    private readonly List<CurrencyRowPresenter> _rows = new List<CurrencyRowPresenter>();

    private bool _isInitialized;

    public CurrencyListPresenter(CurrencyListView view, ViewsFactory views, WalletService wallet)
    {
        _view = view;
        _views = views;
        _wallet = wallet;
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        CurrencyType[] types = _wallet.GetAvailableCurrencies();

        for (int i = 0; i < types.Length; i++)
        {
            CurrencyType type = types[i];

            CurrencyRowView rowView = _views.Create<CurrencyRowView>(ViewIDs.CurrencyRow, _view.Content);
            CurrencyRowPresenter rowPresenter = new CurrencyRowPresenter(rowView, _wallet, type);

            rowPresenter.Initialize();
            _rows.Add(rowPresenter);
        }

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        for (int i = 0; i < _rows.Count; i++)
        {
            CurrencyRowPresenter presenter = _rows[i];
            presenter.Dispose();
            _views.Release(presenter.View);
        }

        _rows.Clear();

        _isInitialized = false;
    }
}