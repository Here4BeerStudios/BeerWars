using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public class ActionMessage : MessageBase
    {
        public uint PlayerId;
        public Vector2Int Origin;
        public ActionCode Code;

        public override void Deserialize(NetworkReader reader)
        {
            PlayerId = reader.ReadPackedUInt32();
            var x = reader.ReadInt32();
            var y = reader.ReadInt32();
            Origin = new Vector2Int(x, y);
            Code = (ActionCode) reader.ReadInt32();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WritePackedUInt32(PlayerId);
            writer.Write(Origin.x);
            writer.Write(Origin.y);
            writer.Write((int) Code);
        }
    }
}
