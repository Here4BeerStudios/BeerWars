using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {
    public ContentHandler ContentHandler;
    public HexCell hexCell;

    private bool _visible;
    private SpriteRenderer[] _renderers;
    private Transform _background;
    private Dictionary<Content, HexCell> _cells;
    private Dictionary<Content, HexCell[]> _build;

    //todo fix raycasting

    void Awake()
    {
        _background = transform.GetChild(0);
        _cells = new Dictionary<Content, HexCell>();
        foreach(var entry in ContentHandler.Contents)
        {
            var cell = Instantiate<HexCell>(hexCell);
            cell.Content = entry.Value;
            cell.transform.SetParent(transform, false);
            _cells.Add(entry.Key, cell);
        }
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        //todo correct
        _build = new Dictionary<Content, HexCell[]>();
        _build.Add(Content.Normal, new[]
        {
            _cells[Content.Brewery],
            _cells[Content.Water],
            _cells[Content.Cornfield],
        });
        EnableRenderers(false);
    }

    public void Use(GameObject obj)
    {
        var origin = obj.GetComponent<HexCell>();
        if (!_visible && _build.ContainsKey(origin.Content))
        {
            var position = obj.transform.localPosition;
            position.z = -1;
            position.y += 2;
            transform.position = position;

            var build = _build[origin.Content];
            var scale = new Vector3(2f * build.Length, 2.5f, 1f);
            _background.localScale = scale;

            position.x = (build.Length - 1) * -1;
            position.y = 0;
            position.z = -0.1f;
            for (var i = 0; i < build.Length; i++)
            {
                build[i].transform.localPosition = position;
                position.x += 2;
            }
            EnableRenderers(true);
        }
        else
        {
            EnableRenderers(false);
        }
    }

    private void EnableRenderers(bool enable)
    {
        _visible = enable;
        foreach(var renderer in _renderers)
        {
            renderer.enabled = enable;
        }
    }
}
