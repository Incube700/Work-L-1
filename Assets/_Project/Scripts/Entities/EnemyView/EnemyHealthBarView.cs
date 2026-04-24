using UnityEngine;
using UnityEngine.UI;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class EnemyHealthBarView : EntityView
{
    [SerializeField] private GameObject _root;
    [SerializeField] private Image _fillImage;

    protected override void Awake()
    {
        base.Awake();

        if (_root == null)
        {
            _root = gameObject;
        }
    }

    protected override void OnBind(Entity entity)
    {
        entity.CurrentHealth.Changed += OnHealthChanged;
        entity.IsDead.Changed += OnIsDeadChanged;

        Refresh();
    }

    protected override void OnUnbind(Entity entity)
    {
        entity.CurrentHealth.Changed -= OnHealthChanged;
        entity.IsDead.Changed -= OnIsDeadChanged;
    }

    private void OnHealthChanged()
    {
        Refresh();
    }

    private void OnIsDeadChanged()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (LinkedEntity == null)
        {
            return;
        }

        bool isDead = LinkedEntity.IsDead.Value;

        if (_fillImage != null)
        {
            float maxHealth = LinkedEntity.MaxHealth.Value;
            float currentHealth = LinkedEntity.CurrentHealth.Value;

            if (maxHealth <= 0f)
            {
                _fillImage.fillAmount = 0f;
            }
            else
            {
                _fillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            }
        }

        if (_root != null)
        {
            _root.SetActive(isDead == false);
        }
    }
}