using UnityEngine;
using Assets._Project.Scripts.Gameplay.EntitiesCore;
using Assets._Project.Scripts.Gameplay.EntitiesCore.Mono;

public abstract class EntityView : MonoBehaviour
{
    private MonoEntity _monoEntity;

    protected Entity LinkedEntity { get; private set; }

    protected virtual void Awake()
    {
        _monoEntity = GetComponentInParent<MonoEntity>();
    }

    protected virtual void Start()
    {
        if (_monoEntity == null)
        {
            return;
        }

        if (_monoEntity.LinkedEntity == null)
        {
            return;
        }

        LinkedEntity = _monoEntity.LinkedEntity;
        OnBind(LinkedEntity);
    }

    protected virtual void OnDisable()
    {
        if (LinkedEntity == null)
        {
            return;
        }

        OnUnbind(LinkedEntity);
        LinkedEntity = null;
    }

    protected abstract void OnBind(Entity entity);

    protected virtual void OnUnbind(Entity entity)
    {
    }
}