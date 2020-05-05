public abstract class Region {

    private int      _Population {get; set;}
    private Entity[] _Members    {get;}

    public Region() {
        _Population = 0;
        _Members    = null;

    }

    public int CountPopulation(Entity[] members) {
        return members.Length;

    }

}
