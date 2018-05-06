using Assets.Scripts.Ingame.Contents;

namespace Assets.Scripts.Ingame.Actions
{
    public class BuildAction : AbstractAction
    {
        public readonly Content Building;

        public BuildAction(PlayerInfo player, HexCell origin, Content building) : base(player, origin)
        {
            Building = building;
        }

        public override string ToString()
        {
            return "Build action, " + base.ToString() + ", Building: " + Building;
        }
    }
}
