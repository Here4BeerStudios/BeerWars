using Assets.Scripts.Ingame.Contents;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public const float OuterRadius = 1;
    public const float InnerRadius = OuterRadius * 0.866025404f;

    public static Vector3[] Corners =
    {
        new Vector3(0f, 0f, OuterRadius),
        new Vector3(InnerRadius, 0f, 0.5f * OuterRadius),
        new Vector3(InnerRadius, 0f, -0.5f * OuterRadius),
        new Vector3(0f, 0f, -OuterRadius),
        new Vector3(-InnerRadius, 0f, -0.5f * OuterRadius),
        new Vector3(-InnerRadius, 0f, 0.5f * OuterRadius),
        new Vector3(0f, 0f, OuterRadius)
    };

    public Vector2Int Pos;

    private SpriteRenderer _backgroundRenderer, _contentRenderer;

    public delegate void Clicked();
    public event Clicked OnClick;

    //TODO correct Player / PlayerInfo
    private TempPlayer _owner;
    public TempPlayer Owner
    {
        get { return _owner; }
        set
        {
            _owner = value;
            _backgroundRenderer.color = value != null ? value.Background : Color.white;
        }
    }

    private CellContent _content;
    public CellContent Content
    {
        get { return _content; }
        set
        {
            _content = value;
            _contentRenderer.sprite = value.Sprite;
        }
    }

    void Awake()
    {
        var child = gameObject.transform.GetChild(0);
        _backgroundRenderer = GetComponent<SpriteRenderer>();
        _contentRenderer = child.GetComponent<SpriteRenderer>();
    }

    public override string ToString()
    {
        return "Cell: (" + Pos.x + ", " + Pos.y + "), Content: " + Content.Key;
    }

    public void OnMouseDown()
    {
        OnClick.Invoke();
    }
}