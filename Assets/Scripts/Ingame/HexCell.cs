using System;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public const float outerRadius = 1;
    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] Corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

    private CellContent _content;

    public CellContent Content
    {
        get { return _content; }
        set
        {
            _content = value;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = value.Sprite;
        }
    }

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}