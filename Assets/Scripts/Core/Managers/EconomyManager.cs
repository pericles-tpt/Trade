using UnityEngine;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

public class EconomyManager : MonoBehaviour
{
    Company[]  _Companies;
    Business[] _Businesses;
    Entity[]   _Entities;

    int maxItems = 1000;

    string path = @".\goods.json";

    // CSV
    //string[,] loadedItems;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);

        int businessToCompanyRatio = 100;
        int companyNo = 1000000;
        _Companies = new Company[companyNo];
        _Businesses = new Business[companyNo * businessToCompanyRatio];

        // CSV
        //loadedItems = new string[5, maxItems];
        //string path = @".\goods.csv";
        //ReadCSV(path);

        // JSON
        string jsonString = File.ReadAllText(path);
        Record[] Records = JsonConvert.DeserializeObject<Record[]>(@jsonString);
        foreach (Record record in Records)
        {
            Debug.Log("index: " + record.index+ ", name: " + record.name + ", components: " + record.components[0] + ", category: " + record.category + ", found: " + record.found[0] + ", mass: " + record.mass);
        }

    }

    // CSV
    /*private void ReadCSV(string directory) {
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
    }*/

    public class Record
    {
        public int index;
        public string name;
        public int[] components;
        public string category;
        public int[] found;
        public int mass;
    }

}