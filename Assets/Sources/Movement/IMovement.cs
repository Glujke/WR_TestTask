using UnityEngine;

namespace WR.Movement
{
    public interface IMovement
    {
        void LockControl();
        void UnlockControl();

        void Move(Transform target);
    }
}
