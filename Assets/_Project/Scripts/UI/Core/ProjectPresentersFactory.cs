public sealed class ProjectPresentersFactory
{
    public MessagePopupPresenter CreateMessagePopupPresenter(MessagePopupView view)
    {
        return new MessagePopupPresenter(view);
    }
}