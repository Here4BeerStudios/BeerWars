using UnityEngine;

public class BuildingMenu : MonoBehaviour {
    private SpriteRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    public void Use(GameObject obj)
    {
        _renderer.enabled = !_renderer.enabled;

        var cell = obj.GetComponent<HexCell>();
        var position = obj.transform.localPosition;
        position.z = -2;
        position.x += 2;
        transform.position = position;
    }
}
