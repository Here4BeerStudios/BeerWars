using Assets.Scripts.Network;
using System.Collections.Generic;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using AddPlayerMessage = Assets.Scripts.Network.Messages.AddPlayerMessage;

public class Host
{
    private List<InitPlayer> _players;
    private Dictionary<NetworkConnection, uint> _netIds;

    public Host(int port)
    {
        //Setup server
        NetworkServer.Listen(port);
        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        NetworkServer.RegisterHandler(BwMsgTypes.LoadScene, OnLoadScene);
        NetworkServer.RegisterHandler(BwMsgTypes.LoadPlayers, OnLoadPlayers);
        NetworkServer.RegisterHandler(BwMsgTypes.Action, OnAction);

        _players = new List<InitPlayer>();
        _netIds = new Dictionary<NetworkConnection, uint>();

        Debug.Log("Server created at port " + port);
    }

    private void OnAddPlayer(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<AddPlayerMessage>();

        var playerId = (uint) _players.Count;
        var newPlayer = new InitPlayer(playerId, msg.Name, GetSpawnColor(), GetSpawnPos());

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

    private void OnLoadScene(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(BwMsgTypes.InitScene, new EmptyMessage());
    }

    private void OnLoadPlayers(NetworkMessage netMsg)
    {
        var playerArray = _players.ToArray();
        foreach (var entry in _netIds)
        {
            entry.Key.Send(BwMsgTypes.InitPlayers, new InitPlayersMessage
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