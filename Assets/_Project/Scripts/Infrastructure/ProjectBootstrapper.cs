using UnityEngine;

public sealed class ProjectBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        if (ProjectContext.Instance == null)
        {
            GameObject context = new GameObject("@ProjectContext");
            context.AddComponent<ProjectContext>();
        }
    }

    private void Start()
    {
        ProjectContext.Instance.Container.Resolve<SceneLoader>().LoadMenu();
    }
}