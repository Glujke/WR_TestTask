
using Mirror;
using System.Collections.Generic;

namespace WR.Network.Info
{
    public struct PlayersConnectionsInfo : NetworkMessage
    {
        public List<PlayerConnectionInfo> infos;
    }
}
