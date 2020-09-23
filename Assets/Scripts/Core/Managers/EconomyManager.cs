using UnityEngine;
using System.IO;

public class EconomyManager : MonoBehaviour
{
    Company[]  _Companies;
    Business[] _Businesses;
    Entity[]   _Entities;

    int maxItems = 1000;

    string[,] loadedItems;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);

        loadedItems = new string[5, maxItems];

        int businessToCompanyRatio = 100;
        int companyNo = 1000000;
        _Companies = new Company[companyNo];
        _Businesses = new Business[companyNo * businessToCompanyRatio];

        string path = @".\goods.csv";
        
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < maxItems; j++)
            {
                loadedItems[i, j] = "asdfghjklmasdfghjklmasdfghjklmasdfghjklmasdfghjklm";
            }
        }
        //ReadCSV(path);

        // necessity * composite/atomic * 

    }

    private void ReadCSV(string directory) {
        int i = 0;        
        using (var reader = new StreamReader(directory))
        {
            // Read past the first line since that just contains headers
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                loadedItems[0, i] = values[0];
                loadedItems[1, i] = values[1];
                loadedItems[2, i] = values[2].Trim('"');
                loadedItems[3, i] = values[3];
                loadedItems[4, i] = values[4].Trim('"');
                    
                Debug.Log("Item no: " + loadedItems[0, i] + ", name: " + loadedItems[1, i] + ", dependencies: " + loadedItems[2, i] + ", category: " + loadedItems[3, i] + ", found: " + loadedItems[4, i]);

                i++;
            }
        }
    }

}