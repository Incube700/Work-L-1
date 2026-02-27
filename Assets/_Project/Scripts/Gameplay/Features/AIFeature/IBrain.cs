using System;

namespace Assets._Project.Scripts.Gameplay.Features.AIFeature
{
    public interface IBrain : IDisposable
    {
        void Enable();
        void Disable();
        void Update(float deltaTime);
    }
}