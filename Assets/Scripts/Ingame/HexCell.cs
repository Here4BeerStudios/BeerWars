using System;
using UnityEngine;
using Random = System.Random;

public enum Ground
{
    Default,
    Cornfield,
    Water,
    Hill
}

public enum Building
{
    Brewery,
    Bar,
    Cornsilo
}

public class HexCell : MonoBehaviour {
    public const float outerRadius = 1;
    public const float innerRadius = outerRadius * 0.866025404f;
    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

    void Awake()
    {
        
    } 

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
