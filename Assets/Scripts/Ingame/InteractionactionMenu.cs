using System;
using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Ingame.Actions;
using System.Collections.Generic;
using UnityEngine;

struct Interaction
{
    public readonly CellContent[] Contents;
    public readonly Func<PlayerInfo, HexCell, Content, AbstractAction> OnClick;

    public Interaction(CellContent[] contents, Func<PlayerInfo, HexCell, Content, AbstractAction> onClick)
    {
        Contents = contents;
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
    private Dictionary<Content, Interaction> _interactions;
    private List<HexCell> _cells;

    void Start()
    {
        _background = transform.GetChild(0);
        _backgroundRenderer = _background.GetComponent<SpriteRenderer>();
        _backgroundRenderer.enabled = false;
        _cells = new List<HexCell>();
        _interactions = new Dictionary<Content, Interaction>();
        InitInteractions();
    }

    private void InitInteractions()
    {
        _interactions.Add(Content.Normal, new Interaction(new []
        {
            ContentHandler[Content.Brewery]
        }, (player, origin, content) => new BuildAction(player, origin.Pos, content)));

        _interactions.Add(Content.Village, new Interaction(new[]
        {
            ContentHandler[Content.Delivery]
        }, (player, origin, content) => new DeliveryAction(player, origin.Pos)));
    }

    public void Use(HexCell origin)
    {
        if (!_backgroundRenderer.enabled && _interactions.ContainsKey(origin.Content))
        {
            var position = origin.transform.localPosition;
            position.z = -1;
            position.y += 2;
            transform.position = position;

            var interaction = _interactions[origin.Content];
            var len = interaction.Contents.Length;
            var scale = new Vector3(2f * len, 2.5f, 1f);
            _background.localScale = scale;
            
            position.x = (len - 1) * -1;
            position.y = 0;
            position.z = -0.1f;
            foreach (var content in interaction.Contents)
            {
                var cell = Instantiate<HexCell>(HexCell);
                cell.Content = content;
                cell.OnClick += () =>
                {
                    Controller.RegisterAction(interaction.OnClick(Controller.Player.PlayerInfo, origin, content));
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
