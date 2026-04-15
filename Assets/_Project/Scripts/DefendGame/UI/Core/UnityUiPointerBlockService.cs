using UnityEngine.EventSystems;

public sealed class UnityUiPointerBlockService : IUiPointerBlockService
{
    public bool IsPointerOverUi()
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        return EventSystem.current.IsPointerOverGameObject();
    }
}