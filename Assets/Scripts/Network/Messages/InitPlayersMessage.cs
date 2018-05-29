using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public class InitPlayer
    {
        public uint NetId;
        public string Name;

        //public Sprite Emblem;
        public Color Background;
        public Vector2Int SpawnPos;
        
        public InitPlayer(uint netId, string name, Color background, Vector2Int spawnPos)
        {
            NetId = netId;
            Name = name;
            Background = background;
            SpawnPos = spawnPos;
        }
    }

    public class InitPlayersMessage : MessageBase
    {
        public uint OwnId;

        public InitPlayer[] Playerses;

        public override void Deserialize(NetworkReader reader)
        {
            OwnId = reader.ReadPackedUInt32();
            var len = reader.ReadInt32();
            Playerses = new InitPlayer[len];
            for (var i = 0; i < len; i++)
            {
                var netId = reader.ReadPackedUInt32();
                var name = reader.ReadString();
                    //todo player emblem?
                var background = reader.ReadColor();
                var spawnPos = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
                Playerses[i] = new InitPlayer(netId, name, background, spawnPos);
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(OwnId);
            writer.Write(Playerses.Length);
            foreach (var player in Playerses)
            {
                writer.WritePackedUInt32(player.NetId);
                writer.Write(player.Name);
                writer.Write(player.Background);
                writer.Write(player.SpawnPos.x);
                writer.Write(player.SpawnPos.y);
                //todo player emblem?
            }
        }
    }
}