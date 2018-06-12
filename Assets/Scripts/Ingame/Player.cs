using UnityEngine;

public class Player {
    public uint NetId;
	public string Name;
    public Sprite Emblem;
    public Color Background;
	public Vector2Int SpawnPos;

	public Player(uint netId, string name, string emblem, Color background, Vector2Int spawnPos)
	{
		NetId = netId;
		Name = name;
		Emblem = Resources.Load("Sprites/Emblems/" + emblem, typeof(Sprite)) as Sprite;
		Background = background;
		SpawnPos = spawnPos;
	}

	public Player(uint netId, string name, Sprite emblem, Color background, Vector2Int spawnPos)
	{
		NetId = netId;
		Name = name;
		Emblem = emblem;
		Background = background;
		SpawnPos = spawnPos;
	}
}
