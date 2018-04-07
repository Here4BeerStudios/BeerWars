using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
    public HexCell hexCell;
    public int width = 6;
    public int height = 6;

    void Awake()
    {
        for (int y = 0, i = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                CreateCell(x, y, i++);
            }
        }
    }

    private void CreateCell(int x, int y, int i)
    {
        var position = new Vector3((x + y * 0.5f - y / 2) * (HexCell.innerRadius * 2f),
            y * (HexCell.outerRadius * 1.5f), 0f);

        var cell = Instantiate<HexCell>(hexCell);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
    }

    void Update()
    {
        //zoom
        var camera = Camera.main;
        var zoom = Input.GetAxis("Mouse ScrollWheel") * -2f;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + zoom, 1f, 100f);
    }
}
