using System;
using System.Collections.Generic;
using Assets._Project.Scripts.UI.Common;

public sealed class DefendGameplayScreenPresenter : IPresenter
{
    private readonly DefendGameplayScreenView _screenView;
    private readonly List<IPresenter> _childPresenters = new List<IPresenter>();

    private bool _isInitialized;

    public DefendGameplayScreenPresenter(
        DefendGameplayScreenView screenView,
        DefendHudPresenter hudPresenter,
        CurrencyListPresenter currencyListPresenter,
        PlacementPanelPresenter placementPanelPresenter,
        DefendResultPresenter resultPresenter)
    {
        _screenView = screenView ?? throw new ArgumentNullException(nameof(screenView));

        _childPresenters.Add(hudPresenter ?? throw new ArgumentNullException(nameof(hudPresenter)));
        _childPresenters.Add(currencyListPresenter ?? throw new ArgumentNullException(nameof(currencyListPresenter)));
        _childPresenters.Add(placementPanelPresenter ?? throw new ArgumentNullException(nameof(placementPanelPresenter)));
        _childPresenters.Add(resultPresenter ?? throw new ArgumentNullException(nameof(resultPresenter)));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        for (int i = 0; i < _childPresenters.Count; i++)
        {
            _childPresenters[i].Initialize();
        }

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        for (int i = _childPresenters.Count - 1; i >= 0; i--)
        {
            _childPresenters[i].Dispose();
        }

        _isInitialized = false;
    }
}