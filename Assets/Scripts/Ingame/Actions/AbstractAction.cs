namespace Assets.Scripts.Ingame.Actions
{
    public abstract class AbstractAction
    {
        public readonly PlayerInfo PlayerInfo;
        public readonly HexCell Origin;

        public AbstractAction(PlayerInfo playerInfo, HexCell origin)
        {
            PlayerInfo = playerInfo;
            Origin = origin;
        }

        public override string ToString()
        {
            return "Player: " + PlayerInfo.Name + ", Origin: (" + Origin.ToString() + ")";
        }
    }
}
