using Assets.Scripts.Core.Data;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    Company[] _Companies;
    Business[] _Businesses;
    Entity[] _Entities;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        int businessToCompanyRatio = 100;
        int companyNo = 1000000;
        _Companies = new Company[companyNo];
        _Businesses = new Business[companyNo * businessToCompanyRatio];

        GenerateMarketEntities();
    }

    public void GenerateMarketEntities()
    {

    }

    public float CalculateItemValue(float supplyDemandRatio, int basePrice)
    {
        return (basePrice / supplyDemandRatio);
    }

    public void GenerateProperties()
    {
        GalaxyManager gm = GameObject.Find("Camera").GetComponent<GalaxyManager>();
        int[] planetSA = new int[gm.noPlanets];
        int million = 1000000;
        for (int i = 0; i < gm.noPlanets; i++)
        {
            switch (gm.GetPlanetByIndex(i)._Scale)
            {
                case (1):
                    planetSA[i] = 350 * million;
                    break;
                case (1.5f):
                    planetSA[i] = 500 * million;
                    break;
                case (2):
                    planetSA[i] = 800 * million;
                    break;
            }
            int j;
            int areaAllowance = planetSA[i];
            while (areaAllowance > 0)
            {

            }
        }
    }
}