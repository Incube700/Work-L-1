using System;

public sealed class DefendUiRuntime : IDisposable
{
    private readonly DefendGameplayScreenPresenter _screenPresenter;
    private readonly PopupService _popupService;

    private bool _isInitialized;

    public DefendUiRuntime(
        DefendGameplayScreenPresenter screenPresenter,
        PopupService popupService)
    {
        _screenPresenter = screenPresenter ?? throw new ArgumentNullException(nameof(screenPresenter));
        _popupService = popupService ?? throw new ArgumentNullException(nameof(popupService));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _screenPresenter.Initialize();

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _screenPresenter.Dispose();
        _popupService.Dispose();

        _isInitialized = false;
    }
}