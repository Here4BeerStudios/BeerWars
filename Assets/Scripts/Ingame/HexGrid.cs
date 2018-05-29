using Assets.Scripts.Ingame.Contents;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public InteractionactionMenu InteractionactionMenu;
    public HexCell HexCell;

    public int Width = 50;
    public int Height = 50;

    private ContentHandler _contents;
    private HexCell[,] _cells;

    void Awake()
    {
        _contents = ContentHandler.Self;
        _cells = new HexCell[Height, Width];
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
        for (int y = 0, i = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var r = Random.value;
                var entry = _contents[r < 0.2 ? Content.Village : r < 0.4 ? Content.Cornfield : r < 0.6 ? Content.Water : Content.Normal];
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

        cell.Pos = new Vector2Int(x, y);
        cell.Content = content;
        cell.OnClick += () => InteractionactionMenu.Use(cell);

        _cells[y, x] = cell;
    }
}