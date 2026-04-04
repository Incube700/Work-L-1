using System;
using Assets._Project.Scripts.UI.Common;

public sealed class DefendUiRuntime : IDisposable
{
    private readonly IPresenter _hudPresenter;
    private readonly IPresenter _currencyListPresenter;
    private readonly IPresenter _resultPresenter;
    private readonly PopupService _popupService;

    private bool _isInitialized;

    public DefendUiRuntime(
        DefendHudPresenter hudPresenter,
        CurrencyListPresenter currencyListPresenter,
        DefendResultPresenter resultPresenter,
        PopupService popupService)
    {
        _hudPresenter = hudPresenter ?? throw new ArgumentNullException(nameof(hudPresenter));
        _currencyListPresenter = currencyListPresenter ?? throw new ArgumentNullException(nameof(currencyListPresenter));
        _resultPresenter = resultPresenter ?? throw new ArgumentNullException(nameof(resultPresenter));
        _popupService = popupService ?? throw new ArgumentNullException(nameof(popupService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _hudPresenter.Initialize();
        _currencyListPresenter.Initialize();
        _resultPresenter.Initialize();

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _resultPresenter.Dispose();
        _currencyListPresenter.Dispose();
        _hudPresenter.Dispose();
        _popupService.Dispose();

        _isInitialized = false;
    }
}