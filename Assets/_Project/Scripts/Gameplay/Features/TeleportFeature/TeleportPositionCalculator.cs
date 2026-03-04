using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public static class TeleportPositionCalculator
    {
        public static Vector3 GetRandomPoint(Entity entity)
        {
            Vector3 from = entity.Transform.position;
            Vector2 offset2 = Random.insideUnitCircle * entity.TeleportRadius;
            Vector3 offset3 = new Vector3(offset2.x, 0f, offset2.y);

            return from + offset3;
        }

        public static bool CanTeleportTowardsCurrentTarget(Entity entity)
        {
            return TryGetPointTowardsCurrentTarget(entity, out _);
        }

        public static bool TryGetPointTowardsCurrentTarget(Entity entity, out Vector3 position)
        {
            Entity target = entity.CurrentTarget.Value;

            if (target == null)
            {
                position = default;
                return false;
            }

            Vector3 from = entity.Transform.position;
            Vector3 to = target.Transform.position;

            Vector3 direction = to - from;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
            {
                position = default;
                return false;
            }

            float distance = direction.magnitude;
            float step = Mathf.Min(distance, entity.TeleportRadius);

            position = from + direction.normalized * step;
            return true;
        }

        public static Vector3 GetPointTowardsCurrentTarget(Entity entity)
        {
            Entity target = entity.CurrentTarget.Value;

            Vector3 from = entity.Transform.position;
            Vector3 to = target.Transform.position;

            Vector3 direction = to - from;
            direction.y = 0f;

            float distance = direction.magnitude;
            float step = Mathf.Min(distance, entity.TeleportRadius);

            return from + direction.normalized * step;
        }
    }
}
