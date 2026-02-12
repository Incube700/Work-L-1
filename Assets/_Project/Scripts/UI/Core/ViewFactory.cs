using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ViewsFactory
{
    private readonly Dictionary<string, string> _paths = new Dictionary<string, string>();
    private readonly Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

    public ViewsFactory()
    {
        _paths.Add(ViewIDs.MessagePopup, "UI/Popups/MessagePopupView");
    }

    public T Create<T>(string viewId, Transform parent) where T : MonoBehaviour
    {
        if (string.IsNullOrEmpty(viewId))
            throw new ArgumentException("View id is empty.", nameof(viewId));

        if (parent == null)
            throw new ArgumentNullException(nameof(parent));

        GameObject prefab = LoadPrefab(viewId);
        GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);

        T view = instance.GetComponent<T>();
        if (view == null)
            throw new InvalidOperationException($"Prefab '{viewId}' has no component '{typeof(T).Name}'.");

        return view;
    }

    public void Release(MonoBehaviour view)
    {
        if (view == null)
            return;

        UnityEngine.Object.Destroy(view.gameObject);
    }

    private GameObject LoadPrefab(string viewId)
    {
        if (_cache.TryGetValue(viewId, out GameObject cached))
            return cached;

        if (_paths.TryGetValue(viewId, out string path) == false)
            throw new InvalidOperationException($"View id is not registered: {viewId}");

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
            throw new MissingReferenceException($"Prefab not found at Resources/{path}");

        _cache.Add(viewId, prefab);
        return prefab;
    }
}