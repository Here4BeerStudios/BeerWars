using System.Collections.Generic;
using Assets.Scripts.Ingame.Contents;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public InteractionactionMenu InteractionactionMenu;
    public HexCell HexCell;

    public int Width;
    public int Height;

    private ContentHandler _contents;
    private HexCell[,] _cells;

    public Dictionary<Vector2Int, int> OccupyRadius;

    void Awake()
    {
        _contents = ContentHandler.Self;
        _cells = new HexCell[Height, Width];
        OccupyRadius = new Dictionary<Vector2Int, int>();
    }

    public HexCell this[int x, int y]
    {
        get { return _cells[y, x]; }
    }

    /// <summary>
    /// Initializes the Grid
    /// </summary>
    public void Init()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var r = Random.value;
                var entry = _contents[r < 0.2 ? Content.Village : r < 0.4 ? Content.Cornfield : r < 0.6 ? Content.Water : Content.Normal];
                CreateCell(x, y, entry);
            }
        }
    }


    public void Load(Content[,] data)
    {
        Height = data.GetLength(0);
        Width = data.GetLength(1);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var entry = _contents[data[y, x]];
                CreateCell(x, y, entry);
            }
        }
    }

    private void CreateCell(int x, int y, CellContent content)
    {
		if (_cells [y, x] != null) {
			Destroy (_cells [y, x].gameObject);
			Destroy (_cells [y, x]);
			// Wrong game initialized.
		}
        var position = new Vector3((x + y * 0.5f - y / 2) * (HexCell.InnerRadius * 2f),
            y * (HexCell.OuterRadius * 1.5f), 0f);

        var cell = Instantiate<HexCell>(HexCell);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        var pos = new Vector2Int(x, y);
        cell.Pos = pos;
        cell.Content = content;
        cell.OnClick += () => InteractionactionMenu.Use(cell);

        _cells[y, x] = cell;

        if (content == Content.Village)
        {
			OccupyRadius[pos] = 0;
        }
    }
}