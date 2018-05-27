using UnityEngine;

public class Player {
    public uint NetId;
	public PlayerInfo PlayerInfo;
    public Color Background;

    public Player(uint netId, PlayerInfo playerInfo, Color background)
    {
        NetId = netId;
        PlayerInfo = playerInfo;
        Background = background;
    }
}
