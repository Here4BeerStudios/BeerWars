using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public class AddPlayerMessage : MessageBase
    {
        public string Name;
        public string Emblem;

        public override void Deserialize(NetworkReader reader)
        {
			Name = reader.ReadString();
			Emblem = reader.ReadString();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(Name);
			writer.Write(Emblem);
        }
    }
}
