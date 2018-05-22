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
        //TODO correct background
        Grid[1, 1].Owner = tempPlayer;
        Grid[2, 1].Owner = tempPlayer;
        Grid[1, 2].Owner = tempPlayer;
        Grid[2, 2].Owner = tempPlayer;
        Grid[3, 2].Owner = tempPlayer;
        Grid[1, 3].Owner = tempPlayer;
        Grid[2, 3].Owner = tempPlayer;
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