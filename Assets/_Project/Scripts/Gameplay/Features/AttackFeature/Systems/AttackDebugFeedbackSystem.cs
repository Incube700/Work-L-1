using System;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Systems;
using Assets._Project.Scripts.Gameplay.Features.MovementFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.AttackFeature
{
    public sealed class AttackDebugFeedbackSystem : IInitializableSystem, IDisposableSystem
    {
        private const float RayLength = 3f;
        private const float RayDuration = 0.5f;
        private const float RotationEpsilonSqr = 0.0001f;

        private Entity _entity;

        private IDisposable _startSub;
        private IDisposable _requestSub;
        private IDisposable _shootSub;
        private IDisposable _endSub;
        private IDisposable _cancelSub;

        public void OnInit(Entity entity)
        {
            _entity = entity;

            Debug.Log("[AttackDebug] Initialized");

            _requestSub = entity.StartAttackRequest.Subscribe(OnRequest);
            _startSub = entity.StartAttackEvent.Subscribe(OnStart);
            _shootSub = entity.AttackDelayEndEvent.Subscribe(OnShoot);
            _endSub = entity.EndAttackEvent.Subscribe(OnEnd);
            _cancelSub = entity.AttackCanceledEvent.Subscribe(OnCancel);
        }

        public void OnDispose()
        {
            _requestSub?.Dispose();
            _startSub?.Dispose();
            _shootSub?.Dispose();
            _endSub?.Dispose();
            _cancelSub?.Dispose();

            _requestSub = null;
            _startSub = null;
            _shootSub = null;
            _endSub = null;
            _cancelSub = null;

            _entity = null;
        }

        private void OnRequest()
        {
            Debug.Log("[Attack] Request");
        }

        private void OnStart()
        {
            Debug.Log("[Attack] Start");
        }

        private void OnShoot()
        {
            Debug.Log("[Attack] ShootMoment");

            if (_entity == null)
                return;

            if (TryResolveOrigin(out Vector3 origin) == false)
            {
                Debug.Log("[Attack] ShootMoment skipped: no transform source");
                return;
            }

            Vector3 dir = ResolveDirection();
            Debug.DrawRay(origin, dir.normalized * RayLength, Color.red, RayDuration);
        }

        private void OnEnd()
        {
            Debug.Log("[Attack] End");
        }

        private void OnCancel()
        {
            Debug.Log("[Attack] Canceled");
        }

        private bool TryResolveOrigin(out Vector3 origin)
        {
            origin = Vector3.zero;

            if (_entity.TryGetComponent(out TransformComponent transformComponent) && transformComponent.Value != null)
            {
                origin = transformComponent.Value.position + Vector3.up;
                return true;
            }

            if (_entity.TryGetComponent(out RigidbodyComponent rigidbodyComponent) && rigidbodyComponent.Value != null)
            {
                origin = rigidbodyComponent.Value.position + Vector3.up;
                return true;
            }

            if (_entity.TryGetComponent(out CharacterControllerComponent controllerComponent) && controllerComponent.Value != null)
            {
                origin = controllerComponent.Value.transform.position + Vector3.up;
                return true;
            }

            return false;
        }

        private Vector3 ResolveDirection()
        {
            if (_entity.TryGetComponent(out RotationDirection rotationDirection) &&
                rotationDirection.Value != null &&
                rotationDirection.Value.Value.sqrMagnitude >= RotationEpsilonSqr)
            {
                return rotationDirection.Value.Value;
            }

            if (_entity.TryGetComponent(out TransformComponent transformComponent) && transformComponent.Value != null)
                return transformComponent.Value.forward;

            if (_entity.TryGetComponent(out RigidbodyComponent rigidbodyComponent) && rigidbodyComponent.Value != null)
                return rigidbodyComponent.Value.transform.forward;

            if (_entity.TryGetComponent(out CharacterControllerComponent controllerComponent) && controllerComponent.Value != null)
                return controllerComponent.Value.transform.forward;

            return Vector3.forward;
        }
    }
}
