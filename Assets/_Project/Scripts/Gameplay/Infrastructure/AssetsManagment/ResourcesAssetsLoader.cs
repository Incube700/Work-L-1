using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Infrastructure.AssetsManagment
{
    public class ResourcesAssetsLoader
    {
        public T Load<T>(string resourcePath) where T : Object
            => Resources.Load<T>(resourcePath);
    }
}