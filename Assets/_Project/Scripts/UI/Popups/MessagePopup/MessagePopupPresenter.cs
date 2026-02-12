public sealed class MessagePopupPresenter : PopupPresenterBase
{
    private readonly MessagePopupView _view;

    private string _title;
    private string _message;

    public MessagePopupPresenter(MessagePopupView view)
    {
        _view = view;
    }

    protected override PopupViewBase View => _view;

    public void SetText(string title, string message)
    {
        _title = title;
        _message = message;
    }

    public override void Initialize()
    {
        _view.SetTitle(_title);
        _view.SetMessage(_message);
    }
}