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

    private bool _isDisposed;

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
        ThrowIfDisposed();

        MessagePopupView view = _viewsFactory.Create<MessagePopupView>(ViewIDs.MessagePopup, _popupLayer);

        MessagePopupPresenter presenter = _presentersFactory.CreateMessagePopupPresenter(view);
        presenter.SetText(title, message);

        OnPopupCreated(presenter, view, closedCallback);

        return presenter;
    }

    public void ClosePopup(PopupPresenterBase popup)
    {
        if (_isDisposed)
        {
            return;
        }

        if (popup == null)
        {
            return;
        }

        if (_presenterToInfo.TryGetValue(popup, out PopupInfo info) == false)
        {
            return;
        }

        popup.CloseRequest -= ClosePopup;

        popup.Hide(() =>
        {
            info.ClosedCallback?.Invoke();

            popup.Dispose();
            _viewsFactory.Release(info.View);
            _presenterToInfo.Remove(popup);
        });
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        List<PopupPresenterBase> popups = new List<PopupPresenterBase>(_presenterToInfo.Keys);

        for (int i = 0; i < popups.Count; i++)
        {
            PopupPresenterBase popup = popups[i];

            if (_presenterToInfo.TryGetValue(popup, out PopupInfo info) == false)
            {
                continue;
            }

            popup.CloseRequest -= ClosePopup;
            popup.Dispose();
            _viewsFactory.Release(info.View);
        }

        _presenterToInfo.Clear();
        _isDisposed = true;
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

    private void ThrowIfDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(PopupService));
        }
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