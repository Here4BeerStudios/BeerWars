﻿using Assets.Scripts.Ingame;
using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    public ContentHandler ContentHandler;
    public HexGrid Grid;
    public ResourceHandler PlayerResource;

    public uint PlayerId;
    private NetHandler _netHandler;
    private Player[] _players;

    public Player LocalPlayer
    {
        get { return _players[PlayerId]; }
    }

    void Start()
    {
        _netHandler = NetHandler.self;
        _netHandler.RegisterHandler(BwMsgTypes.InitPlayers, OnInit);
        _netHandler.RegisterHandler(BwMsgTypes.Action, OnAction);
    }

    private void OnInit(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<InitMessage>();
        //init grid
        //todo grid?
        Grid.Init();

        //init players
        PlayerId = msg.OwnId;
        var initPlayers = msg.Playerses;
        _players = new Player[initPlayers.Length];
        for (int i = 0; i < initPlayers.Length; i++)
        {
            var initPlayer = initPlayers[i];
            //todo check emblem
            var player = new Player(initPlayer.NetId, initPlayer.Name, initPlayer.Background, initPlayer.SpawnPos);
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

        Debug.Log("Initialized Game");
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

    /// <summary>
    /// Sends an action to the server
    /// </summary>
    /// <param name="origin">Origin cell position</param>
    /// <param name="code">Action Code</param>
    public void SendAction(Vector2Int origin, ActionCode code)
    {
        _netHandler.SendAction(new ActionMessage
        {
            PlayerId = PlayerId,
            Origin = origin,
            Code = code
        });
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

        //update resources
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

        //define area of effect
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

        //update area of effect
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
}