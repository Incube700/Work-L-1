using UnityEngine;

public sealed class MageLookAtPointerView : MonoBehaviour
{
    [SerializeField] private Transform _visual;
    [SerializeField] private float _rotationSpeed = 360f;

    private Camera _camera;
    private Plane _plane;

    private void Awake()
    {
        _camera = Camera.main;

        if (_visual == null)
        {
            _visual = transform;
        }
    }

    private void Update()
    {
        if (_visual == null || _camera == null)
        {
            return;
        }

        _plane = new Plane(Vector3.up, _visual.position);

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float enter) == false)
        {
            return;
        }

        Vector3 hitPoint = ray.GetPoint(enter);
        Vector3 direction = hitPoint - _visual.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _visual.rotation = Quaternion.RotateTowards(
            _visual.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime);
    }
}