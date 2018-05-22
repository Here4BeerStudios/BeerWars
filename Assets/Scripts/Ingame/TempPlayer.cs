using UnityEngine;

//TODO replace with Player / PlayerInfo
public class TempPlayer
{
    public readonly string Name;
    public readonly Sprite Emblem;
    public readonly Color Background;

    public TempPlayer(string name, Sprite emblem, Color background)
    {
        Name = name;
        Emblem = emblem;
        Background = background;
    }
}