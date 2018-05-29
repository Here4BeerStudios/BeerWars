using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{
    public class LoadGridMessage : MessageBase
    {
        public int Width, Height;
    }
}
