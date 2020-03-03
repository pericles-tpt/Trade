using System;

public class Item {

    public enum condition              { broken, poor, damaged, good, excellent, mint };
    public long      _IGProductNumber {get; set;}
    private long      _IGSerialNumber  {get; set;}
    private Item[]    _Components;
    private condition _Condition       {get; set;}
    public float      _Value            {get; private set;} // Note: this value is hidden from the player
    public string    _Name            {get; private set;}

    public struct itemProperties
    {
        public long pn { get; }
        public long sn { get; }
        public condition cond { get; }
        public string name { get; }
        public bool hasComponents { get; }
        public Item[] components { get; }

        public itemProperties(long Npn, long Nsn, condition Ncond, string Nname, bool NhasComponents, Item[] Ncomponents)
        {
            pn = Npn;
            sn = Nsn;
            cond = Ncond;
            name = Nname;
            hasComponents = NhasComponents;
            components = Ncomponents;
        }


    }

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

    public float CalculateValue(condition c, long pn) {
        // TODO: Look elsewhere to find market value for the item USING THE "pn"
        // e.g. float inVal = searchDBValue(pn);

        // Ensuring value of item found using pn is round to 2dp
        float inVal     = 10.20f;
        //float outVal    = Math.Round(inVal, 2);

        // Calculating condition multiplier for value of item
        float condMulti = CalculateCondMulti(c);

        // Finally multiply the outVal * condMulti to arrive at final return value
        return inVal * condMulti;

    }

    public float CalculateCondMulti (condition provided) {
        //return (((int)provided + 1) * 0.25);
        return 1f;

    }

}
