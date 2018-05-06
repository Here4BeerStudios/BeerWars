using Assets.Scripts.Ingame.Actions;
using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public Player Player;
	public List<Player> _players;
    public Queue<AbstractAction> _queue; //TODO Threadsafe?

    void Awake()
    {
        //TODO correct player handling
        Player = new Player
        {
            PlayerInfo = new PlayerInfo()
        };

        _players = new List<Player>();
        _queue = new Queue<AbstractAction>();
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void RegisterAction(AbstractAction action)
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

    private void HandleAction(AbstractAction action)
    {
        Debug.Log(action.ToString());
        //todo correct handling
    }
}
