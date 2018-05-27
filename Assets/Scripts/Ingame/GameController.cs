using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using AddPlayerMessage = Assets.Scripts.Network.Messages.AddPlayerMessage;

public class GameController : MonoBehaviour
{
    public ContentHandler ContentHandler;
    public HexGrid Grid;
    public ResourceHandler PlayerResource;
    public PlayerInfo PlayerInfo;

    public uint PlayerId;
    public PlayerInfo LocalPlayerInfo;

    private NetworkClient _netClient;
    private Player[] _players;

    public Player LocalPlayer
    {
        get { return _players[PlayerId]; }
    }

    void Awake()
    {
        _netClient = new NetworkClient();
        _netClient.RegisterHandler(BwMsgTypes.Init, OnInit);
        _netClient.RegisterHandler(BwMsgTypes.Action, OnAction);
        _netClient.RegisterHandler(MsgType.Connect, msg =>
        {
            Debug.Log("Connected to server");

            _netClient.Send(MsgType.AddPlayer, new AddPlayerMessage()
            {
                Name = LocalPlayerInfo.Name,
            });

            _netClient.Send(MsgType.Ready, new EmptyMessage());
        });
    }

    void Start()
    {
        _netClient.Connect("127.0.0.1", 7777);
    }

    private void Spawn(Player player, int x, int y)
    {
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

        Grid[x1, y - 1].Owner = player;
        Grid[x1, y - 1].Content = ContentHandler[Content.Water];
        Grid[x2, y - 1].Owner = player;
        Grid[x2, y - 1].Content = ContentHandler[Content.Normal];
        Grid[x - 1, y].Owner = player;
        Grid[x - 1, y].Content = ContentHandler[Content.Water];
        Grid[x, y].Owner = player;
        Grid[x, y].Content = ContentHandler[Content.Brewery];
        Grid[x + 1, y].Owner = player;
        Grid[x + 1, y].Content = ContentHandler[Content.Cornfield];
        Grid[x1, y + 1].Owner = player;
        Grid[x1, y + 1].Content = ContentHandler[Content.Normal];
        Grid[x2, y + 1].Owner = player;
        Grid[x2, y + 1].Content = ContentHandler[Content.Cornfield];

        PlayerResource.CornFields = 2;
        PlayerResource.WaterFields = 2;
        PlayerResource.Breweries = 1;
    }

    private void Occupy(Player player, int x, int y)
    {
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

        //todo check occupy level
        var cells = new[]
        {
            Grid[x1, y - 1],
            Grid[x2, y - 1],
            Grid[x - 1, y],
            Grid[x, y],
            Grid[x + 1, y],
            Grid[x1, y + 1],
            Grid[x2, y + 1],
        };

        foreach (var cell in cells)
        {
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
        }
    }

    private void OnInit(NetworkMessage netMsg)
    {
        Debug.Log("Initialize Game");
        var msg = netMsg.ReadMessage<InitMessage>();
        PlayerId = msg.OwnId;
        var initPlayers = msg.Players;
        _players = new Player[initPlayers.Length];
        for (int i = 0; i < initPlayers.Length; i++)
        {
            var initPlayer = initPlayers[i];
            var info = Instantiate(PlayerInfo);
            info.Name = initPlayer.Name;
            //todo check emblem
            _players[i] = new Player(initPlayer.NetId, info, initPlayer.Background);
        }

        //todo grid?


        Grid.Init();
        Spawn(LocalPlayer, 2, 2);
    }

    private void OnAction(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ActionMessage>();
        var statePlayer = _players[msg.PlayerId];
        var pos = msg.Origin;
        switch (msg.Code)
        {
            case ActionCode.BuildBrewery:
            {
                // TODO delay
                Grid[pos.x, pos.y].Content = ContentHandler[Content.Brewery];
                //TODO clarify inform user
                break;
            }

            case ActionCode.Delivery:
            {
                // TODO delay
                Occupy(statePlayer, pos.x, pos.y);
                // TODO update other controllers
                break;
            }
        }
    }

    public void SendAction(Vector2Int origin, ActionCode code)
    {
        var msg = new ActionMessage
        {
            PlayerId = PlayerId,
            Origin = origin,
            Code = code
        };
        _netClient.Send(BwMsgTypes.Action, msg);
    }
}