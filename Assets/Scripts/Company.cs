using UnityEditor;
using UnityEngine;

public class Company
{
    public string    _Name          { get; private set; }
    public long      _NetWorth      { get; private set; }
    public int       _SharePrice    { get; private set; }
    public Company[] _Subsidiaries  { get; private set; }
    // public DataTree CompanyStructure { get; private set; } // Is a tree of roles in the company conveying company structure with role names and entities that have those roles includes CEO
    // Assets
    // Properties
    // Income
    // Expenditure
    // WriteOffs

}