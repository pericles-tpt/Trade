using Microsoft.Win32;
using UnityEditor;
using UnityEngine;
public enum assetCategory { property, stock, good };

public class Company
{
    public string    _Name             { get; private set; }
    public long      _NetWorth         { get; private set; }
    public int       _SharePrice       { get; private set; }
    public Company[] _Subsidiaries     { get; private set; }
    public Node      _CompanyStructure { get; private set; } // Is a tree of roles in the company conveying company structure with role names and entities that have those roles includes CEO
    public Asset[]   _Assets           { get; private set; }
    // Income - Calculated during turn?
    // Expenditure - Calculated during turn?
    // WriteOffs - Calculated during turn?
}

public struct Node
{
    public string name;
    public string title;
    public int salary;
    Node[] worksFor;
}

public struct Asset
{
    public int index;
    public assetCategory category;
    public int quantity;
    public int locationIndex;
    public string itemID;
}