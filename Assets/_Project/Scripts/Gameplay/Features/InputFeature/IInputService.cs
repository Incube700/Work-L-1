using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.InputFeature
{
    public interface IInputService
    {
        bool IsEnabled { get; set; }
        
        Vector3 Direction { get; }
        float MouseX { get; }
        
        bool FireDown { get; }
        bool FireUp { get; }

        void Tick();
    }
}