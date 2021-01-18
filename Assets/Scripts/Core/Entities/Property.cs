using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Core.Entities
{
    public enum zoning { residential, commercial, industrial }
    public class Property : MonoBehaviour
    {

        public string location;
        public int size;
        public Building[] buildings;
        public zoning zone;

    }
}