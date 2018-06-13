using System;
using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using Assets.Scripts.Ingame;
using Assets.Scripts.Network;
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
    public GameController Controller;
    public HexCell HexCell;
	public AudioSource brewerySound;
	public AudioSource beerDeliverySound;
    public bool Visible {get; private set; }

    private ContentHandler _contents;
    private Transform _background;
    private SpriteRenderer _backgroundRenderer;
    private ResourceHandler _resources;
    private Dictionary<Content, InteractionGroup> _interactions;
    private List<HexCell> _cells;

    void Start()
    {
        _contents = ContentHandler.Self;
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
        _interactions.Add(Content.Normal, new InteractionGroup(cell => cell.Owner == Controller.LocalPlayer, new []
        {
            new ConcreteInteraction(_contents[Content.Brewery], cell =>
            {
                if (_resources.Beer >= ResourceHandler.BreweryBeerCost)
                {
                    _resources.Beer -= ResourceHandler.BreweryBeerCost;
                    _resources.Breweries += 1;
                    Controller.SendAction(cell.Pos, ActionCode.BuildBrewery);
					brewerySound.Play();
                }
            }), 
        }));
        //delivery action
        _interactions.Add(Content.Village, new InteractionGroup(cell => cell.Owner == null || cell.Owner == Controller.LocalPlayer, new []
        {
            new ConcreteInteraction(_contents[Content.Delivery], cell =>
            {
                var radius = cell.Owner == Controller.LocalPlayer ? Controller.Grid.OccupyRadius[cell.Pos] : 0;
                var cost = (radius == 0 ? 9 : 6 * (radius + 1)) * ResourceHandler.VillageBeerCost;
                if (_resources.Beer >= cost)
                {
                    _resources.Beer -= cost;
                    Controller.SendAction(cell.Pos, ActionCode.Delivery);
					beerDeliverySound.Play();
                }
            }), 
        }));
    }

    /// <summary>
    /// Shows or hides interaction-menu for current cell
    /// </summary>
    /// <param name="origin">origin cell</param>
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
