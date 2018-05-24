using Assets.Scripts.Ingame.Actions;
using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ContentHandler ContentHandler;
    public HexGrid Grid;
    public Player Player;
    public ResourceHandler PlayerResource;

    private Player[] _players;
    private Queue<State> _queue;

    void Awake()
    {
        _players = new Player[10]; //todo check max players
        _queue = new Queue<State>();
    }

    public void RegisterState(State state)
    {
        _queue.Enqueue(state);
    }

    void Start()
    {
        Player.ID = 1;
        _players[1] = Player;

        Grid.Init();
        Spawn(Player, 2, 2);
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
            if (player == Player)
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

    void Update()
    {
        while (_queue.Count > 0)
        {
            //TODO validate state
            HandleState(_queue.Dequeue());
        }
    }

    private void HandleState(State state)
    {
        var statePlayer = _players[state.PlayerId];
        var pos = state.Origin;
        switch (state.Code)
        {
            case ActionCode.BUILD_BREWERY:
            {
                // TODO update resources
                if (PlayerResource.Beer >= ResourceHandler.BreweryBeerCost)
                {
                    Grid[pos.x, pos.y].Content = ContentHandler[Content.Brewery];
                    // TODO delay
                    PlayerResource.Beer -= ResourceHandler.BreweryBeerCost;
                    PlayerResource.Breweries += 1;
                    // TODO update other controllers
                }

                //TODO clarify inform user
                break;
            }

            case ActionCode.DELIVERY:
            {
                if (PlayerResource.Beer >= ResourceHandler.VillageBeerCost)
                {
                    // TODO delay
                    PlayerResource.Beer -= ResourceHandler.VillageBeerCost;
                    //todo handle correct player
                    Occupy(statePlayer, pos.x, pos.y);
                    // TODO update other controllers
                }
                break;
            }
        }
    }
}