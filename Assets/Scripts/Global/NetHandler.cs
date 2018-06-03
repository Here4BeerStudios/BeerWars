using Assets.Scripts.Network;
using Assets.Scripts.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using AddPlayerMessage = Assets.Scripts.Network.Messages.AddPlayerMessage;

public class NetHandler
{
    private static NetHandler _self;
    public static NetHandler Self
    {
        get
        {
            if (_self == null)
                _self = new NetHandler();
            return _self;
        }
    }
    
    public string IpAddress { get; private set; }
    public int Port { get; private set; }
    public bool Online { get; private set; }

    private NetworkClient _netClient;
    private Host _host;

    private NetHandler()
    {
        Online = false;
        _netClient = new NetworkClient();
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
            Name = LocalPlayerInfo.self.Name,
        });
    }

    public void LoadScene()
    {
        _netClient.Send(BwMsgTypes.LoadScene, new EmptyMessage());
    }

    public void LoadGrid(int width, int height)
    {
        _netClient.Send(BwMsgTypes.LoadGrid, new LoadGridMessage
        {
            Height = height,
            Width = width
        });
    }

    public void LoadPlayers()
    {
        _netClient.Send(BwMsgTypes.LoadPlayers, new EmptyMessage());
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
