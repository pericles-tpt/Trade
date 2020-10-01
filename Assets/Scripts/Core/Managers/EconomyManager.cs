using Assets.Scripts.Core.Data;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    Company[]  _Companies;
    Business[] _Businesses;
    Entity[]   _Entities;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        int businessToCompanyRatio = 100;
        int companyNo = 1000000;
        _Companies = new Company[companyNo];
        _Businesses = new Business[companyNo * businessToCompanyRatio];

    }

}