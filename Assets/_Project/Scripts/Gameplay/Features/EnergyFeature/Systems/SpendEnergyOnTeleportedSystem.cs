using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.EnergyFeature
{
    public sealed class SpendEnergyOnTeleportedSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _currentEnergy;
        private float _energyCost;

        private SimpleEvent _teleportedEvent;

        public void OnInit(Entity entity)
        {
            _currentEnergy = entity.CurrentEnergy;
            _energyCost = entity.TeleportEnergyCost;

            _teleportedEvent = entity.TeleportedEvent;
            _teleportedEvent.Invoked += OnTeleported;
        }

        public void OnDispose()
        {
            if (_teleportedEvent != null)
                _teleportedEvent.Invoked -= OnTeleported;

            _currentEnergy = null;
            _teleportedEvent = null;
        }

        private void OnTeleported()
        {
            _currentEnergy.Value -= _energyCost;
        }
    }
}