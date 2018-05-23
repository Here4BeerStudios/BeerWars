using UnityEngine;

namespace Assets.Scripts.Ingame.Actions
{
    public abstract class AbstractAction
    {
        public readonly PlayerInfo PlayerInfo;
        public readonly Vector2Int Origin;

        public AbstractAction(PlayerInfo playerInfo, Vector2Int origin)
        {
            PlayerInfo = playerInfo;
            Origin = origin;
        }

        public override string ToString()
        {
            return "Player: " + PlayerInfo.Name + ", Origin: (" + Origin.x + ", "+ Origin.y + ")";
        }
    }
}
