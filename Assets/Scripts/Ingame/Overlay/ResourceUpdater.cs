using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUpdater : MonoBehaviour
{
    public ResourceHandler Resources;

    public Text Corn;
    public Text Water;
    public Text Beer;

    public void Update()
    {
        Corn.text = Resources.Corn.ToString(CultureInfo.InvariantCulture);
        Water.text = Resources.Water.ToString(CultureInfo.InvariantCulture);
        Beer.text = Resources.Beer.ToString(CultureInfo.InvariantCulture);
    }
}