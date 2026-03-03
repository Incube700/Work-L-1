using UnityEngine;

namespace Assets._Project.Scripts.Utilities.Timers
{
    public sealed class CooldownTimer
    {
        private readonly float _duration;
        private float _time;

        public CooldownTimer(float duration)
        {
            _duration = Mathf.Max(0f, duration);
        }

        public bool IsFinished => _time >= _duration;

        public void Tick(float deltaTime)
        {
            if (IsFinished)
                return;

            _time = Mathf.Min(_time + Mathf.Max(0f, deltaTime), _duration);
        }

        public void Reset()
        {
            _time = 0f;
        }
    }
}
