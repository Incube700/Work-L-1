using UnityEngine;

public sealed class MineEffectsView : MonoBehaviour
{
    [SerializeField] private Transform _effectsPoint;
    [SerializeField] private GameObject _placementSmokePrefab;

    private void Awake()
    {
        if (_effectsPoint == null)
        {
            _effectsPoint = transform;
        }
    }

    private void Start()
    {
        if (_placementSmokePrefab != null)
        {
            Instantiate(_placementSmokePrefab, _effectsPoint.position, Quaternion.identity);
        }
    }
}