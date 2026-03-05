using UnityEngine;

public interface IPointerService
{
    bool TryGetGroundPoint(out Vector3 point);
}

public sealed class DesktopPointerService : IPointerService
{
    private readonly Camera _camera;
    private readonly LayerMask _groundMask;
    private readonly Plane _groundPlane;

    public DesktopPointerService(Camera camera, LayerMask groundMask, float groundY = 0f)
    {
        _camera = camera;
        _groundMask = groundMask;
        _groundPlane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
    }

    public bool TryGetGroundPoint(out Vector3 point)
    {
        point = default;

        if (_camera == null)
        {
            return false;
        }

        Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

        // Всегда сначала берём точку на игровой плоскости, чтобы клики не "липли" к зданиям/врагам.
        if (_groundPlane.Raycast(ray, out float enter))
        {
            point = ray.GetPoint(enter);
            return true;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask, QueryTriggerInteraction.Ignore) == false)
        {
            return false;
        }

        point = hit.point;
        return true;
    }
}
