using UnityEngine;

public class Player {
    public uint NetId;
	public string Name;
    //public Sprite Emblem;
    public Color Background;
	public Vector2Int SpawnPos;

    public Player(uint netId, string name, Color background, Vector2Int spawnPos)
    {
        NetId = netId;
        Name = name;
        Background = background;
        SpawnPos = spawnPos;
    }
}
