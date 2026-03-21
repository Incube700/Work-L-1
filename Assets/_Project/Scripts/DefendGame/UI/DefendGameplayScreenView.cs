using UnityEngine;

public sealed class DefendGameplayScreenView : MonoBehaviour
{
    [SerializeField] private DefendHudView _hudView;
    [SerializeField] private PopupLayer _popupLayer;
    
    public DefendHudView HudView => _hudView;
    public PopupLayer PopupLayer => _popupLayer;
}