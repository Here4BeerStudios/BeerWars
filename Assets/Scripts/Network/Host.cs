using Assets.Scripts.Network;
using System.Collections.Generic;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;
using AddPlayerMessage = Assets.Scripts.Network.Messages.AddPlayerMessage;

public class Host
{
    private List<InitPlayers> _players;
    private Dictionary<NetworkConnection, uint> _netIds;

    public Host(int port)
    {
        //Setup server
        NetworkServer.Listen(port);
        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        NetworkServer.RegisterHandler(MsgType.Ready, OnStartGame);
        NetworkServer.RegisterHandler(BwMsgTypes.Action, OnAction);

        _players = new List<InitPlayers>();
        _netIds = new Dictionary<NetworkConnection, uint>();

        Debug.Log("Server created at port " + port);
    }

    private void OnAddPlayer(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<AddPlayerMessage>();

        var playerId = (uint) _players.Count;
        var newPlayer = new InitPlayers(playerId, msg.Name, GetSpawnColor(), GetSpawnPos());

        _netIds.Add(netMsg.conn, playerId);
        _players.Add(newPlayer);
    }

    private Color GetSpawnColor()
    {
        return Random.ColorHSV(0, 1, 0, 1, 0, 1);
    }

    private Vector2Int GetSpawnPos()
    {
        var x = Random.Range(1, 99);
        var y = Random.Range(1, 99);
        return new Vector2Int(x, y);
    }

    private void OnStartGame(NetworkMessage netMsg)
    {
        var playerArray = _players.ToArray();
        foreach (var entry in _netIds)
        {
            entry.Key.Send(BwMsgTypes.InitPlayers, new InitMessage
            {
                OwnId = entry.Value,
                Playerses = playerArray,
            });
        }
    }

    private void OnAction(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(BwMsgTypes.Action, netMsg.ReadMessage<ActionMessage>());
    }
}