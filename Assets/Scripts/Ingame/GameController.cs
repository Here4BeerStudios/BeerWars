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

        //TODO correct background
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
    }

    private void Occupy(TempPlayer player, int x, int y, int level)
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
            cell.Owner = tempPlayer;
            if (cell.Content == Content.Cornfield)
            {
                ResourceHandler.Cornsilos += 1;
            } else if (cell.Content == Content.Water)
            {
                ResourceHandler.Waters += 1;
            }
        }
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
                    if (ResourceHandler.Beer >= ResourceHandler.BreweryBeerCost)
                    {
                        // TODO delay
                        ResourceHandler.Beer -= ResourceHandler.BreweryBeerCost;
                        ResourceHandler.Breweries += 1;
                        Grid[origin.x, origin.y].Content = ContentHandler[building];
                        // TODO update other controllers
                    }
                    //TODO clarify inform user

                    break;
                }
            }
        }
        else if (actionType == typeof(DeliveryAction))
        {
            if (ResourceHandler.Beer >= ResourceHandler.VillageBeerCost)
            {
                // TODO delay
                var targetPos = ((DeliveryAction) action).Origin;
                ResourceHandler.Beer -= ResourceHandler.VillageBeerCost;
                //todo handle correct player
                Occupy(tempPlayer, targetPos.x, targetPos.y, 1);
                // TODO update other controllers
            }
        }

        //todo handling
    }
}