using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;

public sealed class MageLookAtPointerView : EntityView
{
    [SerializeField] private Transform _rotationRoot;
    [SerializeField] private float _rotationSpeed = 360f;

    protected override void Awake()
    {
        base.Awake();

        if (_rotationRoot == null)
        {
            _rotationRoot = transform;
        }
    }

    private void Update()
    {
        if (LinkedEntity == null)
        {
            return;
        }

        if (LinkedEntity.HasComponent<RotationDirection>() == false)
        {
            return;
        }

        Vector3 direction = LinkedEntity.RotationDirection.Value;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _rotationRoot.rotation = Quaternion.RotateTowards(
            _rotationRoot.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime);
    }

    protected override void OnBind(Entity entity)
    {
    }
}