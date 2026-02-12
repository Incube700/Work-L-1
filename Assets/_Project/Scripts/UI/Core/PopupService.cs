using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PopupService : IDisposable
{
    private readonly ViewsFactory _viewsFactory;
    private readonly ProjectPresentersFactory _presentersFactory;
    private readonly Transform _popupLayer;

    private readonly Dictionary<PopupPresenterBase, PopupInfo> _presenterToInfo =
        new Dictionary<PopupPresenterBase, PopupInfo>();

    public PopupService(
        ViewsFactory viewsFactory,
        ProjectPresentersFactory presentersFactory,
        Transform popupLayer)
    {
        _viewsFactory = viewsFactory ?? throw new ArgumentNullException(nameof(viewsFactory));
        _presentersFactory = presentersFactory ?? throw new ArgumentNullException(nameof(presentersFactory));
        _popupLayer = popupLayer ?? throw new ArgumentNullException(nameof(popupLayer));
    }

    public MessagePopupPresenter OpenMessagePopup(string title, string message, Action closedCallback = null)
    {
        MessagePopupView view = _viewsFactory.Create<MessagePopupView>(ViewIDs.MessagePopup, _popupLayer);

        MessagePopupPresenter presenter = _presentersFactory.CreateMessagePopupPresenter(view);
        presenter.SetText(title, message);

        OnPopupCreated(presenter, view, closedCallback);

        return presenter;
    }

    public void ClosePopup(PopupPresenterBase popup)
    {
        if (popup == null || _presenterToInfo.ContainsKey(popup) == false)
        {
            return;
        }

        popup.CloseRequest -= ClosePopup;

        popup.Hide(() =>
        {
            _presenterToInfo[popup].ClosedCallback?.Invoke();

            DisposeFor(popup);
            _presenterToInfo.Remove(popup);
        });
    }

    public void Dispose()
    {
        foreach (PopupPresenterBase popup in _presenterToInfo.Keys)
        {
            popup.CloseRequest -= ClosePopup;
            DisposeFor(popup);
        }

        _presenterToInfo.Clear();
    }

    private void OnPopupCreated(
        PopupPresenterBase popup,
        PopupViewBase view,
        Action closedCallback)
    {
        PopupInfo info = new PopupInfo(view, closedCallback);

        _presenterToInfo.Add(popup, info);

        popup.Initialize();
        popup.Show();

        popup.CloseRequest += ClosePopup;
    }

    private void DisposeFor(PopupPresenterBase popup)
    {
        popup.Dispose();
        _viewsFactory.Release(_presenterToInfo[popup].View);
    }

    private class PopupInfo
    {
        public PopupInfo(PopupViewBase view, Action closedCallback)
        {
            View = view;
            ClosedCallback = closedCallback;
        }

        public PopupViewBase View { get; }
        public Action ClosedCallback { get; }
    }
}