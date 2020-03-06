using System;

public class Item {

    public enum condition              { broken, poor, damaged, good, excellent, mint };
    private long      _IGProductNumber {get; set;}
    private long      _IGSerialNumber  {get; set;}
    private Item[]    _Components;
    private condition _Condition       {get; set;}
    public double     _Value            {get; private set;} // Note: this value is hidden from the player
    private string    _Name            {get; set;}

    public Item(long pn, long sn, condition cond, string name, bool hasComponents, Item[] components = null) {
        _IGProductNumber = pn;
        _IGSerialNumber  = sn;
        _Condition       = cond;
        _Name            = name;
        _Value           = CalculateValue(cond, pn);

    
        if (hasComponents == true) {
            if (components != null) {
                _Components = components;
            } else {
                throw new Exception("If hasComponents is true an item array must be provided");
            }
        }

    }

    public double CalculateValue(condition c, long pn) {
        // TODO: Look elsewhere to find market value for the item USING THE "pn"
        // e.g. float inVal = searchDBValue(pn);
        double inVal     = 10.20d;

        // If the item is composition of other items then add each of their values
        // to the base values of the item
        if (_Components.Length > 0)
        {

            foreach (Item i in _Components) {
                inVal += i._Value;
            }

        }

        // Ensuring value of item found using pn is round to 2dp
        double outVal    = Math.Round(inVal, 2);

        // Calculating condition multiplier for value of item
        double condMulti = CalculateCondMulti(c);

        // Finally multiply the outVal * condMulti to arrive at final return value
        return outVal * condMulti;

    }

    public double CalculateCondMulti (condition provided) {
        return (((int)provided + 1) * 0.25);

    }

}
