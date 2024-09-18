using UnityEngine;

namespace WR.WR_Input
{
    public class JoystickInput : IInput
    {
        private readonly VariableJoystick joystick;

        public JoystickInput(VariableJoystick joystick)
        {
            this.joystick = joystick;
        }

        public Vector2 GetAxis => new(joystick.Direction.x, joystick.Direction.y);

        public bool IsLocked { get; set; }
    }
}
