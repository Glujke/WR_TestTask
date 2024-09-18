using Mirror;
using System.Collections.Generic;
using System.Linq;
using WR.Configs;
using WR.Network.Info;

namespace WR.Network.Server
{
    public class LobbyConnectionHandler
    {
        private readonly int maxPlayers;
        private readonly PlayerConfig playerConfig;
        private readonly GameConfig gameConfig;
        private Dictionary<int, PlayerConnectionInfo> connections = new();
        private LinkedList<PlayerInfo> freeSlots;

        public LobbyConnectionHandler(PlayerConfig playerConfig, GameConfig gameConfig)
        {
            this.playerConfig = playerConfig;
            this.gameConfig = gameConfig;
            freeSlots = new LinkedList<PlayerInfo>(playerConfig.PlayerInfo);
        }

        public bool IsEverybodyReady()
        {
            int countReady = 0;
            foreach(var con in connections.Values)
            {
                if(con.isReady) countReady++;
            }
            return countReady >= gameConfig.MinConnection;
        }

        public void AddConnectionToLobby(NetworkConnectionToClient conn)
        {
            var slot = GetPlayerInfo();
            var connection = new PlayerConnectionInfo()
            {
                id = conn.connectionId,
                name = slot.NameColor, 
                color = slot.Color,
                num = slot.number
            };
            connections.Add(conn.connectionId, connection);
            conn.Send(connection);
            NetworkServer.SendToAll(new PlayersConnectionsInfo()
            { infos = connections.Values.ToList() });
        }

        public void DeleteConnectionFromLobby(NetworkConnectionToClient conn)
        {
            var connection = connections[conn.connectionId];
            freeSlots.AddFirst(playerConfig.PlayerInfo[connection.num]);
            connections.Remove(conn.connectionId);
            NetworkServer.SendToAll(new PlayersConnectionsInfo()
            { infos = connections.Values.ToList() });
        }

        public void ClientUpdateState(NetworkConnectionToClient conn, PlayerConnectionInfo info)
        {

            if (conn.connectionId != info.id) return;
            connections[conn.connectionId] = info;
            NetworkServer.SendToAll(new PlayersConnectionsInfo()
            { infos = connections.Values.ToList() });
        }


        public void UpdateConnections(PlayersConnectionsInfo conn)
        {
            connections.Clear();
            foreach (var con in conn.infos)
            {
                connections.Add(con.id, con);
            }
        }

        private PlayerInfo GetPlayerInfo()
        {
            var color = freeSlots.First.Value;
            freeSlots.Remove(freeSlots.First);
            return color;
        }
    }
}
