using System.Collections.Generic;
using Assets.Scripts.Ingame;
using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    public HexGrid Grid;
    public ResourceHandler PlayerResource;
	public HighscoreHandler HighscoreHandler;
	public UserNav UserNav;
    public uint PlayerId;

    private ContentHandler _contents;
    private NetHandler _netHandler;
    private Player[] _players;

    public Player LocalPlayer
    {
        get { return _players[PlayerId]; }
    }

    void Start()
    {
        _contents = ContentHandler.Self;
        _netHandler = NetHandler.Self;
        if (_netHandler.Online)
        {
            // multiplayer
            _netHandler.RegisterHandler(BwMsgTypes.InitGrid, OnInitGrid);
            _netHandler.RegisterHandler(BwMsgTypes.InitPlayers, OnInitPlayers);
            _netHandler.RegisterHandler(BwMsgTypes.Action, OnAction);
            _netHandler.LoadGrid(50, 50);
        }
        else
        {
            //singleplayer
            _players = new[]
            {
				new Player(0, LocalPlayerInfo.self.Name, LocalPlayerInfo.self.Emblem, Color.green, new Vector2Int(2, 2)),
				//todo bots?
            };
            Grid.Init();
			Spawn(LocalPlayer);
			UserNav.maxX = Grid.Width * HexCell.InnerRadius * 2f;
			UserNav.maxY = Grid.Height * HexCell.OuterRadius * 1.5f;
			HighscoreHandler.InitPlayerScore (_players);
        }
    }

    private void OnInitGrid(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<InitGridMessage>();
		Grid.Load(msg.Grid);
		UserNav.maxX = Grid.Width * HexCell.InnerRadius * 2f;
		UserNav.maxY = Grid.Height * HexCell.OuterRadius * 1.5f;
        Debug.Log("Initialized grid");
        _netHandler.LoadPlayers();
    }

    private void OnInitPlayers(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<InitPlayersMessage>();

        //init players
        PlayerId = msg.OwnId;
        var initPlayers = msg.Playerses;
        _players = new Player[initPlayers.Length];
        for (var i = 0; i < initPlayers.Length; i++)
        {
            var initPlayer = initPlayers[i];
            var player = new Player(initPlayer.NetId, initPlayer.Name, initPlayer.Emblem, initPlayer.Background, initPlayer.SpawnPos);
            _players[i] = player;
            Spawn(player);
        }

        //center cam on spawn pos
        var localPlayer = LocalPlayer;
        var spx = localPlayer.SpawnPos.x;
        var spy = localPlayer.SpawnPos.y;
        var camPos = new Vector3((spx + spy * 0.5f - spy / 2) * (HexCell.InnerRadius * 2f),
            spy * (HexCell.OuterRadius * 1.5f), -10f);
        Camera.main.transform.localPosition = camPos;

		HighscoreHandler.InitPlayerScore (_players);

        Debug.Log("Initialized Players");
    }

    private void OnAction(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ActionMessage>();
        HandleAction(_players[msg.PlayerId], msg.Origin, msg.Code);
    }

    /// <summary>
    /// Sends an action to the server
    /// </summary>
    /// <param name="origin">Origin cell position</param>
    /// <param name="code">Action Code</param>
    public void SendAction(Vector2Int origin, ActionCode code)
    {
        if (_netHandler.Online)
        {
            _netHandler.SendAction(new ActionMessage
            {
                PlayerId = PlayerId,
                Origin = origin,
                Code = code
            });
        }
        else
        {
            HandleAction(LocalPlayer, origin, code);
            //todo bot actions?
        }
    }

    private void HandleAction(Player player, Vector2Int pos, ActionCode code)
    {
        switch (code)
        {
            case ActionCode.BuildBrewery:
            {
                // TODO delay
                Grid[pos.x, pos.y].Content = _contents[Content.Brewery];
                //TODO clarify inform user
                break;
            }

            case ActionCode.Delivery:
            {
                // TODO delay
                Occupy(player, pos);
                // TODO update other controllers
                break;
            }
        }
    }

    private void Spawn(Player player)
    {
        var x = player.SpawnPos.x;
        var y = player.SpawnPos.y;
        int x1, x2;
        if ((y & 1) == 0)
        {
            x1 = x - 1;
            x2 = x;
        }
        else
        {
            x1 = x;
            x2 = x + 1;
        }

        //update grid
        Grid[x1, y - 1].Owner = player;
        Grid[x1, y - 1].Content = _contents[Content.Water];
        Grid[x2, y - 1].Owner = player;
        Grid[x2, y - 1].Content = _contents[Content.Normal];
        Grid[x - 1, y].Owner = player;
        Grid[x - 1, y].Content = _contents[Content.Water];
        Grid[x, y].Owner = player;
        Grid[x, y].Content = _contents[Content.Brewery];
        Grid[x + 1, y].Owner = player;
        Grid[x + 1, y].Content = _contents[Content.Cornfield];
        Grid[x1, y + 1].Owner = player;
        Grid[x1, y + 1].Content = _contents[Content.Normal];
        Grid[x2, y + 1].Owner = player;
        Grid[x2, y + 1].Content = _contents[Content.Cornfield];

        //update resources
        PlayerResource.CornFields = 2;
        PlayerResource.WaterFields = 2;
        PlayerResource.Breweries = 1;
    }

    private void Occupy(Player player, Vector2Int pos)
    {
        var x = pos.x;
        var y = pos.y;

        //update occupy radius
        if (Grid[x, y].Owner != player)
        {
            // new owner
            Grid.OccupyRadius[pos] = 1;
        }
        else
        {
            // stack occupy level
            Grid.OccupyRadius[pos] += 1;
        }
        var radius = Grid.OccupyRadius[pos];

        //define area of effect
        var dirs = new[,]
        {
            {-1, -1},
            {-1, 0},
            {-1, 1},
            {1, 1},
            {1, 0},
            {1, -1},
        };
        var cells = new List<HexCell>();
        if (radius == 1) cells.Add(Grid[x, y]);
        var rx = x + radius;
        var ry = y;
        for (var i = 0; i < 6; i++)
        {
            for (var r = 0; r < radius; r++)
            {
                rx += dirs[i, 0];
                ry += dirs[i, 1];
                if ((ry & 1) == 0 && (i == 0 || i == 2))
                        rx += 1;
                else if ((ry & 1) == 1 && (i == 3 || i == 5))
                        rx -= 1;

                if (rx >= 0 && rx < Grid.Width && ry >= 0 && ry < Grid.Height)
                    cells.Add(Grid[rx, ry]);
            }
        }

        //update area of effect
		int points = 0;
        foreach (var cell in cells)
        {
			if (cell.Owner != player) {
				cell.Owner = player;

	            if (player == LocalPlayer)
	            {
	                if (cell.Content == Content.Cornfield)
	                {
	                    PlayerResource.CornFields += 1;
	                }
	                else if (cell.Content == Content.Water)
	                {
	                    PlayerResource.WaterFields += 1;
	                }
				}
				points++;
			}
        }

		HighscoreHandler.IncScore (player, cells.Count);
    }
}