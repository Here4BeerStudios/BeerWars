using System;
using Assets.Scripts.Ingame;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using AddPlayerMessage = Assets.Scripts.Network.Messages.AddPlayerMessage;

public class NetHandler : MonoBehaviour
{
    public static NetHandler self;

    public PlayerInfo PlayerInfo;
    public string IpAddress { get; private set; }
    public int Port { get; private set; }
    public bool Online { get; private set; }

    private NetworkClient _netClient;
    private Host _host;

    void Awake()
    {
        // We check if we have a player.
        // If we already have a player - destroy the gameObject.
        // Just some insurance if we somehow mess up stuff ;D
        if (self == null)
        {
            DontDestroyOnLoad(gameObject);
            self = this;
            Online = false;
            _netClient = new NetworkClient();
        }
        else if (self != this)
        {
            Destroy(gameObject);
        }
    }
    

    public void Config(string ipAddress, int port)
    {
        Online = true;
        IpAddress = ipAddress;
        Port = port;
        if (ipAddress.Equals("127.0.0.1"))
            _host = new Host(port);

        _netClient.Connect(IpAddress, Port);
    }

    public bool IsHost
    {
        get { return _host != null; }
    }

    public void Join()
    {
        _netClient.Send(MsgType.AddPlayer, new AddPlayerMessage()
        {
            Name = PlayerInfo.Name,
        });
    }

    public void StartGame()
    {
        Debug.Log("Starting game");
        _netClient.Send(MsgType.Ready, new EmptyMessage());
    }

    public void SendAction(ActionMessage msg)
    {
        _netClient.Send(BwMsgTypes.Action, msg);
    }

    public void RegisterHandler(short type, NetworkMessageDelegate handler)
    {
        _netClient.RegisterHandler(type, handler);
    }
}
