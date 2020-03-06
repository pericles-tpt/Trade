namespace STS.Entities
{
    public class Entity
    {

        public enum CanSell { none, buy, sell, both };
        private CanSell  _MarketRole { get; set; }
        private int[]    _DeathRange;
        private Item[]   _Inventory;
        private double   _NetWorth { get; set; }
        private string[] _Name { get; set; }
        private int      _Age { get; set; }
        private int      _ID { get; set; }

        public Entity(int id, string[] name, CanSell marketRole)
        {
            _MarketRole = marketRole;
            _Inventory  = null;
            _NetWorth   = 0;
            _Name       = new string[name.Length];

            int i = 0;
            foreach (string n in name)
            {
                _Name[i] = n;
                i++;
            }

            _ID = id;

        }

        public double CalculateNetWorth(Item[] inv)
        {
            double netWorth = 0;

            foreach (Item i in inv)
            {
                netWorth += i._Value;

            }

            return netWorth;

        }

        public override string ToString()
        {
            string description = "Entity name is:";
            foreach (string s in _Name)
            {
                description += " " + s;
            }

            description += "\nMarket Role: " + _MarketRole.ToString() + "\nNet Worth: " + _NetWorth + "\nAge: " + _Age + "\nID: " + _ID;

            return description;
        }

    }
}