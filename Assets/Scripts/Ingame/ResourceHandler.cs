using System;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    public const float Delay = 3f;
    public const float Intervall = 2f;

    public const float CornIncome = 1f;
    public const float WaterIncome = 1f;
    public const float BeerIncome = 1f;

    public const float BeerCornCost = 1f;
    public const float BeerWaterCost = 1f;
    public const float BreweryBeerCost = 10f;
    public const float VillageBeerCost = 10f;

    public float Corn;
    public float Water;
    public float Beer;

    public int CornFields;
    public int WaterFields;
    public int Breweries;

    //private float _corn;
    //public float Corn
    //{
    //    get { return _corn; }
    //    set
    //    {
    //        if (value >= 0)
    //        {
    //            _corn = value;
    //        }
    //    }
    //}

    //private float _water;
    //public float Water
    //{
    //    get { return _water; }
    //    set
    //    {
    //        if (value >= 0)
    //        {
    //            _water = value;
    //        }
    //    }
    //}

    //private float _beer;
    //public float Beer
    //{
    //    get { return _beer; }
    //    set
    //    {
    //        if (value >= 0)
    //        {
    //            _beer = value;
    //        }
    //    }
    //}

    //private int _cornfields;
    //public int CornFields
    //{
    //    get { return _cornfields; }
    //    set
    //    {
    //        if (value >= 0)
    //        {
    //            _cornfields = value;
    //        }
    //    }
    //}

    //private int _waterfields;
    //public int WaterFields
    //{
    //    get { return _waterfields; }
    //    set
    //    {
    //        if (value >= 0)
    //        {
    //            _waterfields = value;
    //        }
    //    }
    //}

    //private int _breweries;
    //public int Breweries
    //{
    //    get { return _breweries; }
    //    set
    //    {
    //        if (value >= 0)
    //        {
    //            _breweries = value;
    //        }
    //    }
    //}

    void Start()
    {
        InvokeRepeating("UpdateResources", Delay, Intervall);
    }

    private void UpdateResources()
    {
        Corn += CornFields * CornIncome;
        Water += WaterFields * WaterIncome;
        
        var breweries = Math.Min(Math.Min((int) (Corn / BeerCornCost), (int)(Water / BeerWaterCost)), Breweries);
        //TODO clarify inform user
        Beer += breweries * BeerIncome;
        Corn -= breweries * BeerCornCost;
        Water -= breweries * BeerWaterCost;
    }
}