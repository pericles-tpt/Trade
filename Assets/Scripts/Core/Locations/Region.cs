public class Region {

    private int      _Population {get; set;}
    private entity[] _Members    {get;}

    public void Region() {
        _Population = 0;
        _Members    = null;

    }

    public void CountPopulation(Entity[] members) {
        return members.Length();

    }

}
