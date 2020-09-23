public class Entity {

    public enum CanSell            { none, buy, sell, both };
    private CanSell    _MarketRole {get; set;}
    private Item[]     _Inventory;
    private double     _NetWorth   {get; set;}
    private string[]   _Name       {get; set;}
    private int        _ID         {get; set;}

    public Entity(int id, string[] name, CanSell marketRole) {
        _MarketRole = marketRole;
        _Inventory  = null;
        _NetWorth   = 0;
        _Name       = name;
        _ID         = id;

    }

    public double CalculateNetWorth (Item[] inv) {
        double netWorth = 0;

        for (int i = 0; i < inv.Length; i++) {
            netWorth += inv[i]._Value;

        }

        return netWorth;

    }

}
