using System;
using DG.Tweening;

public abstract class PopupPresenterBase : IDisposable
{
    public event Action<PopupPresenterBase> CloseRequest;

    private Action _hideCallback;

    protected abstract PopupViewBase View { get; }

    public virtual void Initialize()
    {
    }

    public virtual void Dispose()
    {
        View.CloseRequest -= OnCloseRequest;
    }

    public void Show()
    {
        View.CloseRequest += OnCloseRequest;
        View.Show();
    }

    public void Hide(Action callback)
    {
        _hideCallback = callback;
        View.CloseRequest -= OnCloseRequest;

        View.Hide().OnComplete(OnHidden);
    }

    protected void RequestClose()
    {
        CloseRequest?.Invoke(this);
    }

    private void OnCloseRequest()
    {
        RequestClose();
    }

    private void OnHidden()
    {
        _hideCallback?.Invoke();
        _hideCallback = null;
    }
}