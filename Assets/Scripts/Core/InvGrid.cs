using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvGrid {

    public int _width   { get; private set; }
    public int _height  { get; private set; }
    public int[,] _grid { get; private set; }

    public InvGrid(int width, int height)
    {
        _grid = new int[width, height];

    }

}
