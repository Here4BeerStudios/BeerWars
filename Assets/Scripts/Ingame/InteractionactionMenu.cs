using System;
using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Ingame.Actions;
using System.Collections.Generic;
using UnityEngine;

//struct Interaction
//{
//    public readonly CellContent[] Contents;
//    public readonly Func<Player, HexCell, Content, State> OnClick;

//    public Interaction(CellContent[] contents, Func<Player, HexCell, Content, State> onClick)
//    {
//        Contents = contents;
//        OnClick = onClick;
//    }
//}

struct Interaction
{
    public readonly CellContent Content;
    public readonly ActionCode Code;

    public Interaction(CellContent content, ActionCode code)
    {
        Content = content;
        Code = code;
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
    private int _playerId;
    private Dictionary<Content, Interaction[]> _interactions;
    private List<HexCell> _cells;

    void Start()
    {
        _playerId = Controller.Player.ID;
        _background = transform.GetChild(0);
        _backgroundRenderer = _background.GetComponent<SpriteRenderer>();
        _backgroundRenderer.enabled = false;
        _cells = new List<HexCell>();
        //_interactions = new Dictionary<Content, Interaction>();
        _interactions = new Dictionary<Content, Interaction[]>();
        InitInteractions();
    }

    private void InitInteractions()
    {
        _interactions.Add(Content.Normal, new []
        {
            new Interaction(ContentHandler[Content.Brewery], ActionCode.BUILD_BREWERY), 
        });
        _interactions.Add(Content.Village, new[]
        {
            new Interaction(ContentHandler[Content.Delivery], ActionCode.DELIVERY),
        });

        //_interactions.Add(Content.Normal, new Interaction(new []
        //{
        //    ContentHandler[Content.Brewery]
        //}, (player, origin, content) => new State(player.ID, origin.Pos, ActionCode.BUILD_BREWERY)));

        //_interactions.Add(Content.Village, new Interaction(new[]
        //{
        //    ContentHandler[Content.Delivery]
        //}, (player, origin, content) => new DeliveryAction(player, origin.Pos)));
    }

    public void Use(HexCell origin)
    {
        if (!_backgroundRenderer.enabled && _interactions.ContainsKey(origin.Content))
        {
            var position = origin.transform.localPosition;
            position.z = -1;
            position.y += 2;
            transform.position = position;

            var interactions = _interactions[origin.Content];
            //var len = interaction.Contents.Length;
            var len = interactions.Length;
            var scale = new Vector3(2f * len, 2.5f, 1f);
            _background.localScale = scale;
            
            position.x = (len - 1) * -1;
            position.y = 0;
            position.z = -0.1f;
            foreach (var interaction in interactions)
            {
                var cell = Instantiate<HexCell>(HexCell);
                cell.Content = interaction.Content;
                cell.OnClick += () =>
                {
                    Controller.RegisterState(new State(_playerId, origin.Pos, interaction.Code));
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
