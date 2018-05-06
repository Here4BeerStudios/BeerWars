using Assets.Scripts.Ingame.Contents;
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

    public int X, Y;

    private SpriteRenderer _renderer;

    public delegate void Clicked();
    public event Clicked OnClick;

    //public CellType Type;

    private CellContent _content;
    public CellContent Content
    {
        get { return _content; }
        set
        {
            _content = value;
            _renderer.sprite = value.Sprite;
        }
    }

    void Awake()
    {
        var child = gameObject.transform.GetChild(0);
        _renderer = child.GetComponent<SpriteRenderer>();
    }

    public override string ToString()
    {
        return "Cell: (" + X + ", " + Y + "), Content: " + Content.Key;
    }

    public void OnMouseDown()
    {
        OnClick.Invoke();
    }
}