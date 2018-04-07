﻿using UnityEngine;

public class HexGrid : MonoBehaviour {
    public HexCell hexCell;
    public int width = 6;
    public int height = 6;

    public float minZoom = 1f;
    public float maxZoom = 15f;
    public float zoomSensitivity = 2f;
    public float xSensitivity = 0.2f;
    public float ySensitivity = 0.2f;

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
    }
}