using Assets._Project.Scripts.Gameplay.Features.InputFeature;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Features.InputFeature
{
    public sealed class DesktopInputService : IInputService
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string MouseXAxisName = "Mouse X";

        public bool IsEnabled { get; set; } = true;

        public Vector3 Direction { get; private set; }
        public float MouseX { get; private set; }

        public bool FireDown { get; private set; }
        public bool FireUp { get; private set; }

        public void Tick()
        {
            if (IsEnabled == false)
            {
                Direction = Vector3.zero;
                MouseX = 0f;
                FireDown = false;
                FireUp = false;
                return;
            }

            Direction = new Vector3(Input.GetAxisRaw(HorizontalAxisName), 0f, Input.GetAxisRaw(VerticalAxisName));
            MouseX = Input.GetAxisRaw(MouseXAxisName);

            FireDown = Input.GetMouseButtonDown(0);
            FireUp = Input.GetMouseButtonUp(0);
        }
    }
}
