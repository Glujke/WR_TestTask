using UnityEngine;

namespace WR.WR_Input
{
    public class KeyboardInput : IInput
    {
        public Vector2 GetAxis => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        public bool IsLocked { get; set; }
    }
}
