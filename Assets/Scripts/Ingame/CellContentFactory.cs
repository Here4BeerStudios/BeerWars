using System.Collections.Generic;
using UnityEngine;

public class CellContentFactory : MonoBehaviour
{
    public Sprite DefaultSprite;
    public Sprite Cornfield;
    public Sprite Water;
    public Sprite Hill;
    public Sprite Forest;
    public Sprite Brewery;
    public Sprite Bar;
    public Sprite Cornsilo;

    public CellContent CreateWater()
    {
        return new CellContent(Water);
    }
}