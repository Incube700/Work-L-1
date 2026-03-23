using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;
using Assets._Project.Scripts.Gameplay.Features.LifeFeature;
using UnityEngine;

public sealed class MineEffectsView : MonoBehaviour
{
    [SerializeField] private MonoEntity _monoEntity;
    [SerializeField] private Transform _effectsPoint;
    [SerializeField] private GameObject _placementSmokePrefab;
    [SerializeField] private GameObject _explosionPrefab;

    private ReactiveVariable<bool> _isDead;
    private bool _isBound;
    private bool _explosionPlayed;

    private void Awake()
    {
        if (_monoEntity == null)
        {
            _monoEntity = GetComponentInParent<MonoEntity>();
        }

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

    private void Update()
    {
        if (_isBound == false)
        {
            TryBind();
        }
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void TryBind()
    {
        if (_monoEntity == null || _monoEntity.LinkedEntity == null)
        {
            return;
        }

        Entity entity = _monoEntity.LinkedEntity;

        if (entity.TryGetComponent<IsDead>(out var isDeadComponent) == false)
        {
            return;
        }

        _isDead = isDeadComponent.Value;
        _isDead.Changed += OnIsDeadChanged;
        _isBound = true;
    }

    private void Unbind()
    {
        if (_isDead != null)
        {
            _isDead.Changed -= OnIsDeadChanged;
            _isDead = null;
        }

        _isBound = false;
    }

    private void OnIsDeadChanged()
    {
        if (_isDead.Value == false || _explosionPlayed)
        {
            return;
        }

        _explosionPlayed = true;

        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, _effectsPoint.position, Quaternion.identity);
        }
    }
}