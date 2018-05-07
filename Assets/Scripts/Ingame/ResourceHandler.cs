using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    public float Delay;
    public float Intervall;

    public float CornIncome;
    public float BeerIncome;

    public float Corn { get; private set; }
    public float Beer { get; private set; }

    public int Cornsilos;
    public int Breweries;

    void Start()
    {
        InvokeRepeating("UpdateResources", Delay, Intervall);
    }

    private void UpdateResources()
    {
        Corn += Cornsilos * CornIncome;
        Beer += Breweries * BeerIncome;

        //Debug.Log("Corn: " + Corn);
        //Debug.Log("Beer: " + Beer);
    }
}