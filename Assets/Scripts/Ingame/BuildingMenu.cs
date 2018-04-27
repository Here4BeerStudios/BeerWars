using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {
    public ContentHandler ContentHandler;
    public HexCell HexCell;

    private bool _visible;
    private Transform _background;
    private SpriteRenderer _backgroundRenderer;
    private Dictionary<Content, CellContent> _contents;
    private Dictionary<Content, CellContent[]> _build;
    private List<HexCell> _cells;

    void Start()
    {
        _background = transform.GetChild(0);
        _backgroundRenderer = _background.GetComponent<SpriteRenderer>();
        _backgroundRenderer.enabled = false;
         _contents = ContentHandler.Contents;
        _build = new Dictionary<Content, CellContent[]>();
        _cells = new List<HexCell>();
        //todo correct
        _build.Add(Content.Normal, new[]
        {
            _contents[Content.Brewery],
            _contents[Content.Water],
            _contents[Content.Cornfield],
        });
    }

    public void Use(GameObject obj)
    {
        var origin = obj.GetComponent<HexCell>();
        if (!_backgroundRenderer.enabled && _build.ContainsKey(origin.Content))
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
            foreach (var content in build)
            {
                var cell = Instantiate<HexCell>(HexCell);
                cell.Content = content;
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = position;
                _cells.Add(cell);
                position.x += 2;
            }
            _backgroundRenderer.enabled = true;
        }
        else
        {
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
            _cells.Clear();
            _backgroundRenderer.enabled = false;
        }
    }
}
