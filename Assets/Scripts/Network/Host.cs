using Assets.Scripts.Network;
using System.Collections.Generic;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;
using AddPlayerMessage = Assets.Scripts.Network.Messages.AddPlayerMessage;

public class Host : MonoBehaviour
{
    private List<InitPlayer> _players;
    private Dictionary<NetworkConnection, uint> _netIds;

    void Awake()
    {
        NetworkServer.Listen(7777);
        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        NetworkServer.RegisterHandler(MsgType.Ready, OnStartGame);
        NetworkServer.RegisterHandler(BwMsgTypes.Action, OnAction);

        _players = new List<InitPlayer>();
        _netIds = new Dictionary<NetworkConnection, uint>();
    }

    private void OnAddPlayer(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<AddPlayerMessage>();

        var playerId = (uint) _players.Count;
        var newPlayer = new InitPlayer(playerId, Color.green, msg.Name);

        _netIds.Add(netMsg.conn, playerId);
        _players.Add(newPlayer);
    }

    private void OnStartGame(NetworkMessage netMsg)
    {
        var playerArray = _players.ToArray();
        foreach (var entry in _netIds)
        {
            entry.Key.Send(BwMsgTypes.Init, new InitMessage
            {
                OwnId = entry.Value,
                Players = playerArray,
            });
        }
    }

    private void OnAction(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(BwMsgTypes.Action, netMsg.ReadMessage<ActionMessage>());
    }
}