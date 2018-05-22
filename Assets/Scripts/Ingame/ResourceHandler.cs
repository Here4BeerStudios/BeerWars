using System;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    public float Delay;
    public float Intervall;

    public float CornIncome;
    public float WaterIncome;
    public float BeerIncome;

    public float BeerCornCost;
    public float BeerWaterCost;
    public float BreweryBeerCost;

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