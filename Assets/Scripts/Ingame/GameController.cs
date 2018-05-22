using Assets.Scripts.Ingame.Actions;
using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player Player;
    public ContentHandler ContentHandler;
    public ResourceHandler ResourceHandler;
    public HexGrid Grid;

    private TempPlayer tempPlayer; //TODO correct Player / PlayerInfo

    private Queue<AbstractAction> _queue; //TODO Threadsafe?

    void Awake()
    {
        Player = new Player()
        {
            PlayerInfo = new PlayerInfo()
        };
        tempPlayer = new TempPlayer("Guest", null, Color.green);
        _queue = new Queue<AbstractAction>();
    }

    public void RegisterAction(AbstractAction action)
    {
        _queue.Enqueue(action);
    }

    void Start()
    {
        Grid.Init();
        Spawn(tempPlayer, 2, 2);
    }

    private void Spawn(TempPlayer player, int x, int y)
    {
        //TODO correct background
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
        Grid[x1, y-1].Owner = player;
        Grid[x2, y-1].Owner = player;
        Grid[x-1, y].Owner = player;
        Grid[x, y].Owner = player;
        Grid[x+1, y].Owner = player;
        Grid[x1, y+1].Owner = player;
        Grid[x2, y+1].Owner = player;
        Grid[x, y].Content = ContentHandler.Contents[Content.Brewery];
    }

    void Update()
    {
        while (_queue.Count > 0)
        {
            //TODO validate action
            HandleAction(_queue.Dequeue());
        }
    }

    private void HandleAction(AbstractAction action)
    {
        Debug.Log(action.ToString());
        var actionType = action.GetType();
        var playerInfo = action.PlayerInfo;
        var origin = action.Origin;
        if (actionType == typeof(BuildAction))
        {
            var building = ((BuildAction) action).Building;
            switch (building)
            {
                case Content.Brewery:
                {
                    // TODO update resources
                    // TODO delay
                    origin.Content = ContentHandler.Contents[building];
                    // TODO update other controllers
                    break;
                }
            }
        }

        //todo handling
    }
}