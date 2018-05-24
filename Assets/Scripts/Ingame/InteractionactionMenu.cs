using System;
using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Ingame.Actions;
using System.Collections.Generic;
using UnityEngine;

struct InteractionGroup
{
    public readonly Predicate<HexCell> CanClick;
    public readonly ConcreteInteraction[] Interactions;

    public InteractionGroup(Predicate<HexCell> canClick, ConcreteInteraction[] interactions)
    {
        CanClick = canClick;
        Interactions = interactions;
    }
}

struct ConcreteInteraction
{
    public readonly CellContent Content;
    public readonly Action<HexCell> OnClick;

    public ConcreteInteraction(CellContent content, Action<HexCell> onClick)
    {
        Content = content;
        OnClick = onClick;
    }
}

public class InteractionactionMenu : MonoBehaviour {
    public ContentHandler ContentHandler;
    public GameController Controller;
    public HexCell HexCell;
    
    public bool Visible {get; private set; }
    private Transform _background;
    private SpriteRenderer _backgroundRenderer;
    //private Dictionary<Content, Interaction> _interactions;
    private Player _player;
    private ResourceHandler _resources;
    private Dictionary<Content, InteractionGroup> _interactions;
    private List<HexCell> _cells;

    void Start()
    {
        _player = Controller.Player;
        _resources = Controller.PlayerResource;
        _background = transform.GetChild(0);
        _backgroundRenderer = _background.GetComponent<SpriteRenderer>();
        _backgroundRenderer.enabled = false;
        _cells = new List<HexCell>();
        _interactions = new Dictionary<Content, InteractionGroup>();
        InitInteractions();
    }

    private void InitInteractions()
    {
        //build actions
        _interactions.Add(Content.Normal, new InteractionGroup(cell => cell.Owner == _player, new []
        {
            new ConcreteInteraction(ContentHandler[Content.Brewery], cell =>
            {
                if (cell.Owner == _player && _resources.Beer >= ResourceHandler.BreweryBeerCost)
                {
                    _resources.Beer -= ResourceHandler.BreweryBeerCost;
                    _resources.Breweries += 1;
                    Controller.RegisterState(new State(_player.ID, cell.Pos, ActionCode.BUILD_BREWERY));
                }
            }), 
        }));
        //delivery action
        _interactions.Add(Content.Village, new InteractionGroup(cell => true, new []
        {
            new ConcreteInteraction(ContentHandler[Content.Delivery], cell =>
            {
                if (_resources.Beer >= ResourceHandler.VillageBeerCost)
                {
                    _resources.Beer -= ResourceHandler.VillageBeerCost;
                    Controller.RegisterState(new State(_player.ID, cell.Pos, ActionCode.DELIVERY));
                }
            }), 
        }));
    }

    public void Use(HexCell origin)
    {
        if (!_backgroundRenderer.enabled && _interactions.ContainsKey(origin.Content))
        {
            var interactionGroup = _interactions[origin.Content];
            if (interactionGroup.CanClick(origin))
            {
                var position = origin.transform.localPosition;
                position.z = -1;
                position.y += 2;
                transform.position = position;

                //var len = interaction.Contents.Length;
                var len = interactionGroup.Interactions.Length;
                var scale = new Vector3(2f * len, 2.5f, 1f);
                _background.localScale = scale;

                position.x = (len - 1) * -1;
                position.y = 0;
                position.z = -0.1f;
                foreach (var interaction in interactionGroup.Interactions)
                {
                    var cell = Instantiate<HexCell>(HexCell);
                    cell.Content = interaction.Content;
                    cell.OnClick += () =>
                    {
                        interaction.OnClick(origin);
                        Reset();
                    };
                    cell.transform.SetParent(transform, false);
                    cell.transform.localPosition = position;
                    _cells.Add(cell);
                    position.x += 2;
                }
                _backgroundRenderer.enabled = true;
                Visible = true;
            }
        }
        else
        {
            Reset();
        }
    }

    private void Reset()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell.gameObject);
        }
        _cells.Clear();
        _backgroundRenderer.enabled = false;
        Visible = false;
    }
}
