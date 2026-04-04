using System;
using Assets._Project.Scripts.UI.Common;

public sealed class DefendResultPresenter : IPresenter
{
    private readonly DefendResultService _resultService;
    private readonly PopupService _popupService;
    private readonly GameFlowService _gameFlowService;
    private readonly DefendLevelConfig _levelConfig;

    private bool _isInitialized;
    private bool _isPopupShown;

    public DefendResultPresenter(
        DefendResultService resultService,
        PopupService popupService,
        GameFlowService gameFlowService,
        DefendLevelConfig levelConfig)
    {
        _resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
        _popupService = popupService ?? throw new ArgumentNullException(nameof(popupService));
        _gameFlowService = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));
        _levelConfig = levelConfig ?? throw new ArgumentNullException(nameof(levelConfig));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _isPopupShown = false;
        _resultService.ResultChanged += OnResultChanged;

        _isInitialized = true;
    }

    public void Dispose()
    {
        if (_isInitialized == false)
        {
            return;
        }

        _resultService.ResultChanged -= OnResultChanged;
        _isInitialized = false;
    }

    private void OnResultChanged(DefendGameResult result)
    {
        if (_isPopupShown)
        {
            return;
        }

        _isPopupShown = true;

        switch (result)
        {
            case DefendGameResult.Win:
                OpenWinPopup();
                break;

            case DefendGameResult.Lose:
                OpenLosePopup();
                break;
        }
    }

    private void OpenWinPopup()
    {
        string title = "Victory";
        string message = $"Base defended.\nReward: {_levelConfig.WinRewardGold} gold.";

        _popupService.OpenMessagePopup(title, message, OnPopupClosed);
    }

    private void OpenLosePopup()
    {
        string title = "Defeat";
        string message = "The base was destroyed.";

        _popupService.OpenMessagePopup(title, message, OnPopupClosed);
    }

    private void OnPopupClosed()
    {
        _gameFlowService.OpenMainMenu();
    }
}