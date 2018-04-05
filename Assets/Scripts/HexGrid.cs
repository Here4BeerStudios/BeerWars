using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
    public HexCell hexCell;
    public Text hexCellLabel;
    public int width = 6;
    public int height = 6;

    HexCell[] cells;
    Canvas gridCanvas;
    HexMesh hexMesh;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexCell.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexCell.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(hexCell);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        Text label = Instantiate<Text>(hexCellLabel);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = x.ToString() + "\n" + z.ToString();
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }
}
