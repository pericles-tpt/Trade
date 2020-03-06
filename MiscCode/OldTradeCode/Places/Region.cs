using STS.Entities;

namespace STS.Places
{
    public class Region
    {

        public int      _Population { get; set; }
        public Entity[] _Members    { get; set; }

        public Region()
        {
            _Population = 0;
            _Members = null;

        }

        public int CountPopulation(Entity[] members)
        {
            return members.Length;

        }

    }
}
