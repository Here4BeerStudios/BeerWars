using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public class AddPlayerMessage : MessageBase
    {
        public string Name;
        //public Sprite Emblem;

        public override void Deserialize(NetworkReader reader)
        {
            Name = reader.ReadString();
            //todo player emblem?
        }

        public override void Serialize(NetworkWriter writer)
        {
                writer.Write(Name);
                //todo player emblem?
        }
    }
}
