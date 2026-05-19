using UnityEngine;
using UnityEngine.UI;

public sealed class PopupLayer : MonoBehaviour
{
    [Header("Optional Generated Skin")]
    [SerializeField] private GameObject _optionalSkinRoot;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _frameImage;
}
