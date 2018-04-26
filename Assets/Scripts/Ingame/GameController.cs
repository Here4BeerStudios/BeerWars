using System.Collections.Generic;
using UnityEngine;

public struct Action
{
    public readonly Player player;
    public readonly HexCell Cell;

    public Action(Player player, HexCell cell)
    {
        this.player = player;
        Cell = cell;
    }
}

public class GameController : MonoBehaviour {
	public List<Player> _players;
    public Queue<Action> _queue; //TODO Threadsafe?

    void Awake()
    {
        _players = new List<Player>();
        _queue = new Queue<Action>();
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void RegisterAction(Action action)
    {
        _queue.Enqueue(action);
    }

	// Use this for initialization
	void Start () {
		// Here init map?

		// Define territories?

		// Some other stuff?
	}
	
	// Update is called once per frame
	void Update () {
        while (_queue.Count > 0)
        {
            HandleAction(_queue.Dequeue());
        }
		// Here do what should be checked every tick.
		// Don't know what exactly. 

		// Probably the action that the players want to do and checking if they can do.
		// Add resources to the players

		// For online mode relevant: Just have 1 gamecontroller | Exists on server.
	}

    private void HandleAction(Action action)
    {
        Debug.Log(action.Cell.ToString());
        //todo correct handling
        switch (action.Cell.CellContent.Content)
        {
            case Content.Normal:
                {

                    break;
                }
            case Content.Cornfield:
                {

                    break;
                }
            case Content.Forest:
                {

                    break;
                }
        }
    }
}
