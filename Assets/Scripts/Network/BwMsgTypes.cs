using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
    public abstract class BwMsgTypes
    {
        public const short Init = MsgType.Highest + 20;
        public const short Action = MsgType.Highest + 50;
    }
}
