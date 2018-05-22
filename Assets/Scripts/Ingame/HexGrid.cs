using Assets.Scripts.Ingame.Contents;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class HexGrid : MonoBehaviour {
    public ContentHandler ContentHandler;
    public BuildingMenu BuildingMenu;
    public HexCell HexCell;

    public int Width = 50;
    public int Height = 50;

    private HexCell[,] _cells;
    private Random _rnd;

    void Awake()
    {
        _cells = new HexCell[Height, Width];
        _rnd = new Random();
    }

    public HexCell this[int x, int y]
    {
        get { return _cells[y, x]; }
    } 

    public void Init()
    {
        for (int y = 0, i = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var r = _rnd.NextDouble();
                var entry = ContentHandler.Contents[
                    r < 0.2 ? Content.Cornfield : r < 0.4 ? Content.Water : Content.Normal];
                CreateCell(x, y, i++, entry);
            }
        }
    }

    private void CreateCell(int x, int y, int i, CellContent content)
    {
        var position = new Vector3((x + y * 0.5f - y / 2) * (HexCell.InnerRadius * 2f),
            y * (HexCell.OuterRadius * 1.5f), 0f);

        var cell = Instantiate<HexCell>(HexCell);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        cell.X = x;
        cell.Y = y;
        cell.Content = content;
        cell.OnClick += () => BuildingMenu.Use(cell);

        _cells[y, x] = cell;
    }
}
