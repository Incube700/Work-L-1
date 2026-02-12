using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupViewBase : MonoBehaviour
{
    public event Action CloseRequest;

    [SerializeField] private CanvasGroup _mainGroup;
    [SerializeField] private Image _anticlicker;
    [SerializeField] private RectTransform _body;

    private float _anticlickerDefaultAlpha;
    private Tween _current;

    protected virtual void Awake()
    {
        if (_mainGroup == null) throw new MissingReferenceException($"{name}: MainGroup is not set.");
        if (_anticlicker == null) throw new MissingReferenceException($"{name}: Anticlicker is not set.");
        if (_body == null) throw new MissingReferenceException($"{name}: Body is not set.");

        _anticlickerDefaultAlpha = _anticlicker.color.a;
        _mainGroup.alpha = 0;
        _mainGroup.blocksRaycasts = false;
        _mainGroup.interactable = false;
    }

    public Tween Show()
    {
        Kill();

        _mainGroup.blocksRaycasts = true;
        _mainGroup.interactable = true;

        Sequence seq = DOTween.Sequence()
            .Append(_anticlicker.DOFade(_anticlickerDefaultAlpha, 0.15f).From(0))
            .Join(_body.DOScale(1f, 0.25f).From(0.8f).SetEase(Ease.OutBack))
            .Join(_mainGroup.DOFade(1f, 0.1f).From(0));

        _current = seq.SetUpdate(true).Play();
        return _current;
    }

    public Tween Hide()
    {
        Kill();

        Sequence seq = DOTween.Sequence()
            .Append(_body.DOScale(0.85f, 0.18f).SetEase(Ease.InBack))
            .Join(_anticlicker.DOFade(0f, 0.15f))
            .Join(_mainGroup.DOFade(0f, 0.15f))
            .OnComplete(OnHidden);

        _current = seq.SetUpdate(true).Play();
        return _current;
    }

    public void OnCloseButtonClicked()
    {
        CloseRequest?.Invoke();
    }

    private void OnHidden()
    {
        _mainGroup.blocksRaycasts = false;
        _mainGroup.interactable = false;
    }

    private void OnDestroy()
    {
        Kill();
    }

    private void Kill()
    {
        if (_current != null)
        {
            _current.Kill();
            _current = null;
        }
    }
}