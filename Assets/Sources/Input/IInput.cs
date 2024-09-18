using UnityEngine;


namespace WR.WR_Input
{
    public interface IInput 
    {
        Vector2 GetAxis { get; }

        bool IsLocked { get; set; }
    }
}
