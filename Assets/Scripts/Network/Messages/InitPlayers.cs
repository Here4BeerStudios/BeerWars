using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public struct InitPlayers
    {
        public uint NetId;
        public string Name;

        //public Sprite Emblem;
        public Color Background;
        public Vector2Int SpawnPos;


        public InitPlayers(uint netId, string name, Color background, Vector2Int spawnPos)
        {
            NetId = netId;
            Name = name;
            Background = background;
            SpawnPos = spawnPos;
        }
    }

    public class InitMessage : MessageBase
    {
        public uint OwnId;

        public InitPlayers[] Playerses;
        //todo grid instance?

        public override void Deserialize(NetworkReader reader)
        {
            OwnId = reader.ReadPackedUInt32();
            var len = reader.ReadInt32();
            Playerses = new InitPlayers[len];
            for (var i = 0; i < len; i++)
            {
                Playerses[i] = new InitPlayers()
                {
                    NetId = reader.ReadPackedUInt32(),
                    Name = reader.ReadString(),
                    Background = reader.ReadColor(),
                    SpawnPos = new Vector2Int(reader.ReadInt32(), reader.ReadInt32()),
                    //todo player emblem?
                };
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