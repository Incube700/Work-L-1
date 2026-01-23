using UnityEngine;

public sealed class ProjectBootstrapper : MonoBehaviour
{
    private ProjectContext _context;

    private void Awake()
    {
        _context = FindFirstObjectByType<ProjectContext>();

        if (_context == null)
        {
            GameObject go = new GameObject("@ProjectContext");
            _context = go.AddComponent<ProjectContext>();
        }

        _context.Initialize();
    }

    private void Start()
    {
        SceneLoader loader = _context.Container.Resolve<SceneLoader>();
        loader.Load(SceneNames.MainMenu);
    }
}