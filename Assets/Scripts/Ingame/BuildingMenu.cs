﻿using Assets.Scripts.Ingame.Contents;
using Assets.Scripts.Ingame.Actions;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {
    public ContentHandler ContentHandler;
    public GameController Controller;
    public HexCell HexCell;
    
    public bool Visible {get; private set; }
    private Transform _background;
    private SpriteRenderer _backgroundRenderer;
    private Dictionary<Content, CellContent[]> _build;
    private List<HexCell> _cells;

    void Start()
    {
        _background = transform.GetChild(0);
        _backgroundRenderer = _background.GetComponent<SpriteRenderer>();
        _backgroundRenderer.enabled = false;
        _build = new Dictionary<Content, CellContent[]>();
        _cells = new List<HexCell>();
        //todo correct
        _build.Add(Content.Normal, new[]
        {
            ContentHandler[Content.Brewery],
            ContentHandler[Content.Water],
            ContentHandler[Content.Cornfield],
        });
    }

    public void Use(HexCell origin)
    {
        if (!_backgroundRenderer.enabled && _build.ContainsKey(origin.Content))
        {
            var position = origin.transform.localPosition;
            position.z = -1;
            position.y += 2;
            transform.position = position;

            var build = _build[origin.Content];
            var scale = new Vector3(2f * build.Length, 2.5f, 1f);
            _background.localScale = scale;
            
            position.x = (build.Length - 1) * -1;
            position.y = 0;
            position.z = -0.1f;
            foreach (var content in build)
            {
                var cell = Instantiate<HexCell>(HexCell);
                cell.Content = content;
                cell.OnClick += () =>
                {
                    Controller.RegisterAction(new BuildAction(Controller.Player.PlayerInfo, origin, content));
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
