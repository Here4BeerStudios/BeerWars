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
    public const float VillageBeerCost = 50f;


    public float Corn;
    public float Water;
    public float Beer;

    public int Cornsilos;
    public int Waters;
    public int Breweries;

    void Start()
    {
        InvokeRepeating("UpdateResources", Delay, Intervall);
    }

    private void UpdateResources()
    {
        Corn += Cornsilos * CornIncome;
        Water += Waters * WaterIncome;
        
        var breweries = Math.Min(Math.Min((int) (Corn / BeerCornCost), (int)(Water / BeerWaterCost)), Breweries);
        //TODO clarify inform user
        Beer += breweries * BeerIncome;
        Corn -= breweries * BeerCornCost;
        Water -= breweries * BeerWaterCost;
    }
}