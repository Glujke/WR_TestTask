using UnityEngine;
using VContainer.Unity;
using WR.Configs;
using WR.Network.Items;
using WR.WR_Input;

namespace WR.Movement
{
    public class PlayerMovement : IMovement
    {
        private readonly IInput inputHandler;
        private readonly PlayerConfig playerConfig;

        public bool IsMoving => inputHandler.GetAxis != Vector2.zero;

        public PlayerMovement(IInput inputHandler, PlayerConfig playerConfig)
        {
            this.inputHandler = inputHandler;
            this.playerConfig = playerConfig;
            UnlockControl();
        }

        public void LockControl()
        {
            inputHandler.IsLocked = true;
        }

        public void UnlockControl()
        {
            inputHandler.IsLocked = false;
        }

        public void Move(Transform target)
        {
            if (!inputHandler.IsLocked && IsMoving)
            {
                var direction = new Vector3(inputHandler.GetAxis.x, 0, inputHandler.GetAxis.y);
                target.Translate(direction * Time.deltaTime * playerConfig.Speed);
            }
        }
    }

}