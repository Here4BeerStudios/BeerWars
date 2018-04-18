using UnityEngine;

public struct CellContent
{
    public readonly Sprite Sprite;
    public readonly CellContent[] Modifikations;

    public CellContent(Sprite sprite, CellContent[] modifikations = null)
    {
        Sprite = sprite;
        Modifikations = modifikations ?? new CellContent[0];
    }
}