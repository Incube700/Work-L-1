using System.Collections.Generic;
using UnityEngine;

public sealed class ConfigService
{
    private readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();

    public T Load<T>(string resourcesPath) where T : Object
    {
        if (string.IsNullOrEmpty(resourcesPath))
        {
            throw new System.ArgumentException("Path is empty.", nameof(resourcesPath));
        }

        if (_cache.TryGetValue(resourcesPath, out Object cached))
        {
            return (T)cached;
        }

        T loaded = Resources.Load<T>(resourcesPath);

        if (loaded == null)
        {
            throw new MissingReferenceException($"Config not found at Resources/{resourcesPath}");
        }

        _cache.Add(resourcesPath, loaded);
        return loaded;
    }
}