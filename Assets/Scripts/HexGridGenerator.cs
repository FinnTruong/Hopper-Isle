using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public static HexGridGenerator Instance;

    public Tile tilePrefab;
    public Tile woodTilePrefab;
    public Tile sandTilePrefab;
    public Tile oceanTilePrefab;

    [Range(0, 1)]
    public float waterThreshold = 0.4f;
    [Range(0, 1)]
    public float sandThreshold = 0.6f;
    [Range(0, 1)]
    public float grassThreshold = 0.8f;

    public int radius;
    public float spacer;

    public List<Tile> tileList = new();

    private Transform mapHolder;

    private float hexWidth;
    private float hexHeight;

    public int seed;
    [Range(0, 1)]
    public float refinement;

    public List<Tile> UnlockedTilesList => tileList.FindAll(x => x.state == TileState.HasUnlocked);

    public int UnlockedTiles => tileList.Count(v => v.state == TileState.HasUnlocked);
    public int TotalTiles => tileList.Count != 0 ? tileList.Count : 1;
    public float MapProgress => (float)UnlockedTiles / TotalTiles;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        seed = Random.Range(0, 1000);
        GenerateGrid();
    }
    public void InitializeGrid()
    {
        hexWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x + spacer;
        hexHeight = tilePrefab.GetComponent<Renderer>().bounds.size.y + spacer;
        string holderName = "Generated Map";
        if (transform.Find(holderName))
            DestroyImmediate(transform.Find(holderName).gameObject);

        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        mapHolder.transform.localPosition = Vector3.zero;
        tileList.Clear();
    }

    public void GenerateGrid()
    {
        InitializeGrid();
        System.Random pseudoRNG = new System.Random(seed);
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (!IsValidCoord(x, y))
                    continue;

                Tile newTile;
                float randHeight = Mathf.PerlinNoise((x + seed) * refinement, (y + seed) * refinement);
                if (randHeight <= waterThreshold)
                {
                    newTile = Instantiate(oceanTilePrefab);
                }

                else if (randHeight <= sandThreshold)
                {
                    newTile = Instantiate(sandTilePrefab);
                }

                else
                    newTile = Instantiate(woodTilePrefab);
                    
                newTile.transform.position = CoordToWorldPos(x, y);
                newTile.name = $"Tile {x},{y}";
                newTile.transform.parent = mapHolder.transform;
                newTile.CreateTile(new Vector2Int(x, y), x == 0 && y == 0);
                tileList.Add(newTile);

            }
        }

        foreach (var tile in tileList)
        {

            Tile targetTile = GetTileFromCoordinate(new Vector2Int(tile.coordinate.x + 1, tile.coordinate.y));
            if (targetTile != null)
                tile.neighborTiles.Add(targetTile);


            targetTile = GetTileFromCoordinate(new Vector2Int(tile.coordinate.x + 1, tile.coordinate.y - 1));
            if (targetTile != null)
                tile.neighborTiles.Add(targetTile);


            targetTile = GetTileFromCoordinate(new Vector2Int(tile.coordinate.x, tile.coordinate.y - 1));
            if (targetTile != null)
                tile.neighborTiles.Add(targetTile);


            targetTile = GetTileFromCoordinate(new Vector2Int(tile.coordinate.x, tile.coordinate.y + 1));
            if (targetTile != null)
                tile.neighborTiles.Add(targetTile);


            targetTile = GetTileFromCoordinate(new Vector2Int(tile.coordinate.x - 1, tile.coordinate.y));
            if (targetTile != null)
                tile.neighborTiles.Add(targetTile);


            targetTile = GetTileFromCoordinate(new Vector2Int(tile.coordinate.x - 1, tile.coordinate.y + 1));
            if (targetTile != null)
                tile.neighborTiles.Add(targetTile);

        }

        UnlockCenter();

    }

    public void UnlockAllTile()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            tileList[i].state = TileState.HasUnlocked;
            tileList[i].UpdateState();
        }
    }

    private void UnlockCenter()
    {
        var centerTile = GetTileFromCoordinate(new Vector2Int(0, 0));
        centerTile.UnlockTile(false);
        foreach (var tile in centerTile.neighborTiles)
        {
            tile.UnlockTile(false);
        }
    }

    public Tile GetTileFromCoordinate(Vector2Int coord)
    {
        if (!IsValidCoord(coord.x, coord.y))
            return null;

        return tileList.FirstOrDefault(v => v.coordinate == coord);
    }

    public Vector2 CoordToWorldPos(int x, int y)
    {
        var tileSize = hexHeight / 2;
        var worldX = Mathf.Sqrt(3) * tileSize * x + Mathf.Sqrt(3)/2 * tileSize * y;
        var worldY = 3 / 2f * tileSize * -y;
        return new Vector2(worldX, worldY);
    }

    public bool IsValidCoord(int x, int y)
    {
        return Mathf.Abs(GetSCoord(x, y)) <= radius;
    }

    public int GetSCoord(int x, int y) => -x - y;


    public void CreateSpecialTile()
    {
        var hiddenTiles = tileList.FindAll(x => x.state == TileState.Hidden);

        var randomTile = hiddenTiles.GetRandomElement();
        while(randomTile.isSpecialTile)
        {
            randomTile = hiddenTiles.GetRandomElement();
        }
        randomTile.isSpecialTile = true;
        randomTile.UpdateState();
    }
}

public enum TileShape
{
    PointyTop = 0,
    FlatTop = 1,
}