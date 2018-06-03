using UnityEngine;

namespace Assets.Scripts.Ingame.Contents
{
    public struct CellContent
    {
        public readonly Content Key;
        public readonly Sprite Sprite;

        public CellContent(Content key, Sprite sprite)
        {
            Key = key;
            Sprite = sprite;
        }

        public static implicit operator Content(CellContent cellContent)
        {
            return cellContent.Key;
        }
    }
}
