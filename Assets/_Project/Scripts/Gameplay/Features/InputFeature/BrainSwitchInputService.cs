using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.InputFeature
{
    public interface IBrainSwitchInputService
    {
        bool RandomPressed { get; }
        bool SmartPressed { get; }

        void Tick();
    }

    public sealed class BrainSwitchInputService : IBrainSwitchInputService
    {
        private readonly KeyCode _randomKey;
        private readonly KeyCode _smartKey;

        public BrainSwitchInputService(KeyCode randomKey, KeyCode smartKey)
        {
            _randomKey = randomKey;
            _smartKey = smartKey;
        }

        public bool RandomPressed { get; private set; }
        public bool SmartPressed { get; private set; }

        public void Tick()
        {
            RandomPressed = Input.GetKeyDown(_randomKey);
            SmartPressed = Input.GetKeyDown(_smartKey);
        }
    }
}
