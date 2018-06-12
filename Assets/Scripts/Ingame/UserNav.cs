using UnityEngine;

public class UserNav : MonoBehaviour
{
    public float minZoom = 1f;
    public float maxZoom = 15f;
    public float zoomSensitivity = 2f;
    public float xSensitivity = 0.2f;
    public float ySensitivity = 0.2f;

    private Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
    }
    
    void Update()
    {
        //zoom
        var zoom = Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
        _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize + zoom, minZoom, maxZoom);
        //moving
        var m = Input.mousePosition;
        var t = _cam.transform.localPosition;
        if (m.x >= Screen.width - 1 || Input.GetKey(KeyCode.D))
        {
            t.x += xSensitivity;
        }
        else if (m.x <= 0 || Input.GetKey(KeyCode.A))
        {
            t.x -= xSensitivity;
        }
        if (m.y >= Screen.height - 1 || Input.GetKey(KeyCode.W))
        {
            t.y += ySensitivity;
        }
        else if (m.y <= 0 || Input.GetKey(KeyCode.S))
        {
            t.y -= ySensitivity;
        }
        _cam.transform.localPosition = t;
    }
}