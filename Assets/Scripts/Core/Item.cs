using System.exception;

public class Item {

    public enum condition              { broken, poor, damaged, good, excellent, mint };
    private long      _IGProductNumber {get; set;}
    private long      _IGSerialNumber  {get; set;}
    private item[]    _Components;
    private condition _Condition       {get; set;}
    private float     _Value           {get; set;} // Note: this value is hidden from the player
    private string    _Name            {get; set;}

    public void Item(long pn, long sn, condition cond, string name, bool hasComponents, item[] components = null) {
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
        float inVal     = 10.20;
        float outVal    = Math.round(val, 2);

        // Calculating condition multiplier for value of item
        float condMulti = CalculateCondMulti(c);

        // Finally multiply the outVal * condMulti to arrive at final return value
        return outVal * condMulti;

    }

    public float CalculateCondMulti (condition provided) {
        return ((provided + 1) * 0.25);

    }

}
