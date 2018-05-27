using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public struct InitPlayer
    {
        public uint NetId;
        public Color Background;

        public string Name;
        //public Sprite Emblem;

        public InitPlayer(uint netId, Color background, string name)
        {
            NetId = netId;
            Background = background;
            Name = name;
        }
    }

    public class InitMessage : MessageBase
    {
        public uint OwnId;

        public InitPlayer[] Players;
        //todo grid instance?

        public override void Deserialize(NetworkReader reader)
        {
            OwnId = reader.ReadPackedUInt32();
            var len = reader.ReadInt32();
            Players = new InitPlayer[len];
            for (var i = 0; i < len; i++)
            {
                Players[i] = new InitPlayer()
                {
                    NetId = reader.ReadPackedUInt32(),
                    Background = reader.ReadColor(),
                    Name = reader.ReadString(),
                    //todo player emblem?
                };
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(OwnId);
            writer.Write(Players.Length);
            foreach (var player in Players)
            {
                writer.WritePackedUInt32(player.NetId);
                writer.Write(player.Background);
                writer.Write(player.Name);
                //todo player emblem?
            }
        }
    }
}