using UnityEngine;

public sealed class BillboardView : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;

    private void LateUpdate()
    {
        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;

            if (_targetCamera == null)
            {
                return;
            }
        }

        transform.LookAt(
            transform.position + _targetCamera.transform.rotation * Vector3.forward,
            _targetCamera.transform.rotation * Vector3.up);
    }
}