using UnityEngine;

public sealed class DefendGameplayScreenView : MonoBehaviour
{
    [SerializeField] private DefendHudView _hudView;
    [SerializeField] private Transform _popupRoot;

    public DefendHudView HudView => _hudView;
    public Transform PopupRoot => _popupRoot;
}