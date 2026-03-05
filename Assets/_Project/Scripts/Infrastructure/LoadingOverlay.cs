using UnityEngine;
using UnityEngine.UI;

public sealed class LoadingOverlay
{
    private readonly GameObject _root;
    private readonly LoadingOverlayUpdater _updater;

    private LoadingOverlay(GameObject root, LoadingOverlayUpdater updater)
    {
        _root = root;
        _updater = updater;
    }

    public static LoadingOverlay Create()
    {
        GameObject root = new GameObject("LoadingOverlay");
        Object.DontDestroyOnLoad(root);

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;

        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();

        GameObject backgroundGo = new GameObject("Background");
        backgroundGo.transform.SetParent(root.transform, false);

        RectTransform bgRect = backgroundGo.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        Image background = backgroundGo.AddComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0.92f);

        GameObject labelGo = new GameObject("Label");
        labelGo.transform.SetParent(backgroundGo.transform, false);

        RectTransform labelRect = labelGo.AddComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0.5f, 0.5f);
        labelRect.anchorMax = new Vector2(0.5f, 0.5f);
        labelRect.sizeDelta = new Vector2(460f, 80f);
        labelRect.anchoredPosition = Vector2.zero;

        Text label = labelGo.AddComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.text = "Loading...";
        label.alignment = TextAnchor.MiddleCenter;
        label.fontSize = 36;
        label.color = Color.white;

        LoadingOverlayUpdater updater = root.AddComponent<LoadingOverlayUpdater>();
        updater.Initialize(label);

        return new LoadingOverlay(root, updater);
    }

    public void Bind(AsyncOperation operation)
    {
        _updater.Bind(operation);
    }

    public void Dispose()
    {
        if (_root != null)
        {
            Object.Destroy(_root);
        }
    }
}

public sealed class LoadingOverlayUpdater : MonoBehaviour
{
    private Text _label;
    private AsyncOperation _operation;

    public void Initialize(Text label)
    {
        _label = label;
    }

    public void Bind(AsyncOperation operation)
    {
        _operation = operation;
    }

    private void Update()
    {
        if (_label == null)
        {
            return;
        }

        if (_operation == null)
        {
            _label.text = "Loading...";
            return;
        }

        float normalizedProgress = Mathf.Clamp01(_operation.progress / 0.9f);
        int percent = Mathf.RoundToInt(normalizedProgress * 100f);
        _label.text = $"Loading {percent}%";
    }
}
