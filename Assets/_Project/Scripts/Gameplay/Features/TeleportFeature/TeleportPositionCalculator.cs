using Assets._Project.Scripts.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.TeleportFeature
{
    public interface ITeleportPositionCalculator
    {
        bool CanCalculate(Entity entity);
        Vector3 Calculate(Entity entity);
    }

    public sealed class RandomTeleportPositionCalculator : ITeleportPositionCalculator
    {
        public bool CanCalculate(Entity entity) => true;

        public Vector3 Calculate(Entity entity)
        {
            Vector3 from = entity.Transform.position;
            Vector2 offset2 = Random.insideUnitCircle * entity.TeleportRadius;
            Vector3 offset3 = new Vector3(offset2.x, 0f, offset2.y);

            return from + offset3;
        }
    }

    public sealed class TargetTeleportPositionCalculator : ITeleportPositionCalculator
    {
        public bool CanCalculate(Entity entity)
        {
            Entity target = entity.CurrentTarget.Value;

            if (target == null)
                return false;

            Vector3 direction = target.Transform.position - entity.Transform.position;
            direction.y = 0f;

            return direction.sqrMagnitude > 0.0001f;
        }

        public Vector3 Calculate(Entity entity)
        {
            Entity target = entity.CurrentTarget.Value;

            if (target == null)
                return entity.Transform.position;

            Vector3 from = entity.Transform.position;
            Vector3 to = target.Transform.position;

            Vector3 direction = to - from;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
                return from;

            float distance = direction.magnitude;
            float step = Mathf.Min(distance, entity.TeleportRadius);

            return from + direction.normalized * step;
        }
    }
}
