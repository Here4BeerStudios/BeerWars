using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
    public abstract class BwMsgTypes
    {
        public const short LoadScene = MsgType.Highest + 11;
        public const short LoadPlayers = MsgType.Highest + 12;

        public const short InitScene = MsgType.Highest + 21;
        public const short InitPlayers = MsgType.Highest + 22;
        public const short InitGrid = MsgType.Highest + 23;

        public const short Action = MsgType.Highest + 50;
    }
}
