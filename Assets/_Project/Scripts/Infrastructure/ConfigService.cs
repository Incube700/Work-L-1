using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ConfigService
{
    private readonly Dictionary<Type, string> _resourcesPaths = new Dictionary<Type, string>();
    private readonly Dictionary<string, UnityEngine.Object> _cache = new Dictionary<string, UnityEngine.Object>();

    public ConfigService()
    {
        _resourcesPaths.Add(typeof(GameModesConfig), "Configs/GameModesConfig");
        _resourcesPaths.Add(typeof(EconomyConfig), "Configs/EconomyConfig");
    }

    public T Load<T>() where T : UnityEngine.Object
    {
        Type type = typeof(T);

        if (_resourcesPaths.TryGetValue(type, out string resourcesPath) == false)
        {
            throw new InvalidOperationException($"Resources path is not registered for config type: {type.Name}");
        }

        return LoadByPath<T>(resourcesPath);
    }

    private T LoadByPath<T>(string resourcesPath) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(resourcesPath))
        {
            throw new ArgumentException("Path is empty.", nameof(resourcesPath));
        }

        if (_cache.TryGetValue(resourcesPath, out UnityEngine.Object cached))
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