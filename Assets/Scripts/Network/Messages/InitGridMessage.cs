using Assets.Scripts.Ingame.Contents;
using UnityEngine.Networking;

namespace Assets.Scripts.Network.Messages
{

    public class InitGridMessage : MessageBase
    {
        public Content[,] Grid;

        public override void Deserialize(NetworkReader reader)
        {
            var height = reader.ReadInt32();
            var width = reader.ReadInt32();
            Grid = new Content[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    Grid[y, x] = (Content) reader.ReadInt32();
                }
            }
        }

        public override void Serialize(NetworkWriter writer)
        {
            var height = Grid.GetLength(0);
            var width = Grid.GetLength(1);
            writer.Write(width);
            writer.Write(height);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    writer.Write((int) Grid[y, x]);
                }
            }
        }
    }
}
