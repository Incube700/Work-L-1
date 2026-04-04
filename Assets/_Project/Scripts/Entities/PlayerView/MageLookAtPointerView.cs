using UnityEngine;

public sealed class MageLookAtPointerView : MonoBehaviour
{
    [SerializeField] private Transform _rotationRoot;
    [SerializeField] private float _rotationSpeed = 360f;

    private DefendInputHandler _inputHandler;
    private Vector3 _targetPoint;
    private bool _hasTargetPoint;
    private bool _isSubscribed;

    public void Construct(DefendInputHandler inputHandler)
    {
        Unsubscribe();

        _inputHandler = inputHandler;
        Subscribe();
    }

    private void Awake()
    {
        if (_rotationRoot == null)
        {
            _rotationRoot = transform;
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Update()
    {
        if (_hasTargetPoint == false)
        {
            return;
        }

        Vector3 direction = _targetPoint - _rotationRoot.position;
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

    private void Subscribe()
    {
        if (_isSubscribed)
        {
            return;
        }

        if (_inputHandler == null)
        {
            return;
        }

        _inputHandler.AimPointChanged += OnAimPointChanged;
        _isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (_isSubscribed == false)
        {
            return;
        }

        if (_inputHandler != null)
        {
            _inputHandler.AimPointChanged -= OnAimPointChanged;
        }

        _isSubscribed = false;
    }

    private void OnAimPointChanged(Vector3 point)
    {
        _targetPoint = point;
        _hasTargetPoint = true;
    }
}