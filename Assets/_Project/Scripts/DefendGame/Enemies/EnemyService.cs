using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Gameplay.EntitiesCore;

public sealed class EnemyService : IDisposable
{
    private readonly List<Entity> _enemies = new List<Entity>();

    public event Action<Entity> EnemyAdded;

    public int AliveCount
    {
        get
        {
            int count = 0;

            for (int i = 0; i < _enemies.Count; i++)
            {
                Entity enemy = _enemies[i];

                if (enemy == null)
                {
                    continue;
                }

                if (enemy.IsDead.Value == false)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public void Add(Entity enemy)
    {
        if (enemy == null)
        {
            throw new ArgumentNullException(nameof(enemy));
        }

        _enemies.Add(enemy);
        EnemyAdded?.Invoke(enemy);
    }

    public void Remove(Entity enemy)
    {
        _enemies.Remove(enemy);
    }

    public void Dispose()
    {
        _enemies.Clear();
        EnemyAdded = null;
    }
}
