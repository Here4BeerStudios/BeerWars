using UnityEngine;

public enum Content
{
    Normal,
    Cornfield,
    Water,
    Forest,
    Hill,
    Brewery,
    Village
}

public struct CellContent
{
    public readonly Content Content;
    public readonly Sprite Sprite;

    public CellContent(Content content, Sprite sprite)
    {
        Content = content;
        Sprite = sprite;
    }
}