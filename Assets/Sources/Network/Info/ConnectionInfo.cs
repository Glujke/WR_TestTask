
using Mirror;
using UnityEngine;

namespace WR.Network.Info
{

    public struct FlagCreateMessage : NetworkMessage
    {
        public int id;
        public int num;
        public Color color;
    }
}
