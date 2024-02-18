using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    //Store the Mesh or Sprite | Add in Inspector
    public GameObject tilePrefab;

    //How big is this gonna be?
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;
    public float spacer = 0.02f; //wild hair addition | add a gap between hexes

    //Store info about the supplied Hex
    private float hexWidth;
    private float hexHeight;
    private Vector3 InitialPosition;    //Place first hex at upper-left corner of grid

    void Start()
    {
        hexWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x + spacer;
        hexHeight = tilePrefab.GetComponent<Renderer>().bounds.size.y + spacer;

        //Center of grid will 0,0,0. Find upper-left position of the first Hex
        InitialPosition = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2,   // x
                                        gridHeightInHexes / 2f * hexHeight - hexHeight / 2, // y
                                        -0.0f);                                             // z  | 2d so just hard code. 
                                                                                            //      If 3d swap y & z
        CreateHexGrid();
    }

    void CreateHexGrid()
    {
        //Create a parent object for all the hexes
        GameObject hexagonGridParent = new GameObject("HexagonGrid");

        //Loop for Hex Rows
        for (int y = 0; y < gridHeightInHexes; y++)
        {
            //Loop for Hex Columns
            for (int x = 0; x < gridWidthInHexes; x++)
            {
                //Create a clone of the supplied Hex object
                GameObject hex = Instantiate(tilePrefab);
  

                //Get the current x,y of loop to place Hex | column 5, row 3, etc.
                Vector2Int gridCoord = new Vector2Int(x, y);

                //Center of grid is 0,0,0 figure out the pixel coordinates of this hex based on it's x,y
                hex.transform.position = CalculateWorldCoordinates(gridCoord);
                var axialCoord = ConvertToAxialCoord(gridCoord.x, gridCoord.y);
                hex.name = $"Tile {axialCoord.x},{axialCoord.y}";
                //Add the hex to the parent ojbect
                hex.transform.parent = hexagonGridParent.transform;
            }
        }
    }

    //Translate an x,y grid position to screen pixel coordinates
    public Vector3 CalculateWorldCoordinates(Vector2 pHexPosition)
    {
        //Hexagons, how do they work? MATHS! 
        //--New rows are moved down 3/4ths of the height (in 2d this is y | in 3d this is z)
        //--and moved left or right by half the width depending on the row.

        //Hex with point-y side up : Every other row is offset by half of the width
        float offset = 0;               //first row normal, alternating rows get the 1/2 bump
        if (pHexPosition.y % 2 != 0)    //even rows = 0
            offset = hexWidth / 2;      //odd rows = width / 2

        //Parenthesis for readablity, not technically needed.
        //Find the x coordinate based on x & width things
        float x = InitialPosition.x + offset + (pHexPosition.x * hexWidth);

        //This is a 2d generator, so offset y for each new row by 3/4ths the hight
        float y = InitialPosition.y - (pHexPosition.y * hexHeight * 0.75f);
        return new Vector3(x, y, 0.2f); //Since 2d just hardcoding a z to whatever works (would be y in 3d)
    }

    private Vector2 ConvertToAxialCoord(int col, int row)
    {
        var q = col - (row - (row & 1)) / 2;
        var r = row;
        return new Vector2(q, r);
    }
}
