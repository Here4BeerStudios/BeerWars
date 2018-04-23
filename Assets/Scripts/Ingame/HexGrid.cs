﻿using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {
    public HexCell hexCell;

    public int width = 6;
    public int height = 6;

    public float minZoom = 1f;
    public float maxZoom = 15f;
    public float zoomSensitivity = 2f;
    public float xSensitivity = 0.2f;
    public float ySensitivity = 0.2f;

    private Dictionary<Content, CellContent> contents;

    void Awake()
    {
        contents = new Dictionary<Content, CellContent>(8);
        LoadCellContents();
        for (int y = 0, i = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                CreateCell(x, y, i++);
            }
        }
    }

    private void LoadCellContents()
    {
        var sprites = Resources.LoadAll<Sprite>(@"Sprites/Cell/Content");
        var spritesDic = new Dictionary<string, Sprite>(sprites.Length);
        foreach (var sprite in sprites)
        {
            spritesDic.Add(sprite.name, sprite);
        }
        //AddContent(Content.Normal, null);
        //AddContent(Content.Cornfield, spritesDic["Cornfield"]);
        AddContent(Content.Water, spritesDic["Water"]);
        //AddContent(Content.Forest, spritesDic["Forest"]);
        //AddContent(Content.Hill, spritesDic["Hill"]);
        //AddContent(Content.Brewery, spritesDic["Brewery"]);
        //AddContent(Content.Village, spritesDic["Village"]);
    }

    private void AddContent(Content content, Sprite sprite)
    {
        contents.Add(content, new CellContent(content, sprite));
    }

    private void CreateCell(int x, int y, int i)
    {
        var position = new Vector3((x + y * 0.5f - y / 2) * (HexCell.innerRadius * 2f),
            y * (HexCell.outerRadius * 1.5f), 0f);

        var cell = Instantiate<HexCell>(hexCell);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        cell.X = x;
        cell.Y = y;
        cell.CellContent = contents[Content.Water];
    }

    void Update()
    {
        var camera = Camera.main;
        //zoom
        var zoom = Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + zoom, minZoom, maxZoom);
        //moving
        var m = Input.mousePosition;
        var t = camera.transform.localPosition;
        if (m.x >= Screen.width)
        {
            t.x += xSensitivity;
        } else if (m.x <= 0)
        {
            t.x -= xSensitivity;
        }
        if (m.y >= Screen.height)
        {
            t.y += ySensitivity;
        }
        else if (m.y <= 0)
        {
            t.y -= ySensitivity;
        }
        camera.transform.localPosition = t;
        // click
        if (Input.GetMouseButtonDown(0))
        {
            // use Raycast for click detection behind content sprite
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null)
            {
                var cell = hit.transform.gameObject.GetComponent<HexCell>();
                Debug.Log(cell.ToString());
            }
        }
    }
}