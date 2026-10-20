using System;
using UnityEngine;
using UnityEngine.Serialization;
using static Constants;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform pieceParent;
    [SerializeField] private Tile tilePrefab;
    
    
    private PrefabLibrary _prefabLibrary;
    private TileMatrix _tileMatrix;

    
    private int _moveCount;

    
    private void Start()
    {
        EventManager.OnMoveSpent += MoveSpentEventHandler;
    }

    private void OnDestroy()
    {
        EventManager.OnMoveSpent -= MoveSpentEventHandler;
    }

    public void Configure(TileMatrix tileMatrix)
    {
        _tileMatrix = tileMatrix;

        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
        
        CreateRandomLevel();
    }

    private void CreateLevelByLevelRepresentation(LevelRepresentation levelRepresentation)
    {
        _moveCount = levelRepresentation.moveCount;
        
        var width = levelRepresentation.gridWidth;
        var height = levelRepresentation.gridHeight;
        var grid = levelRepresentation.grid;
        
        var tiles = new Tile[width, height];

        var vertOffset = height / 2f - 0.5f;
        var horOffset = width / 2f - 0.5f;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = Instantiate(tilePrefab, parent: transform);
                tile.transform.localPosition = new Vector3(i - horOffset, j - vertOffset);
                tile.xIndex = i;
                tile.yIndex = j;

                tiles[i, j] = tile;
                if (j == height - 1)
                {
                    tile.gameObject.AddComponent<GenerationComponent>();
                }

                var index = width * j + i;

                var itemType = GameItemType.RandomCube;

                if (itemType == GameItemType.RandomCube)
                {
                    itemType = (GameItemType) Random.Range(1, 5);
                }
                
                var randomItemPrefab = _prefabLibrary.GetGameItemPrefabFromType(itemType);
                var item = Instantiate(randomItemPrefab, parent: transform);
                
                tile.AttachGameItemToTile(item);   
            }
            _tileMatrix.SetTiles(tiles);
        }
        
        EventManager.LevelStarted(levelRepresentation);
    }

    private void CreateRandomLevel()
    {
        var width = 9;
        var height = 11;
        
        var tiles = new Tile[width, height];

        var vertOffset = height / 2f - 0.5f;
        var horOffset = width / 2f - 0.5f;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = Instantiate(tilePrefab, parent: transform);
                tile.transform.localPosition = new Vector3(i - horOffset, j - vertOffset);
                tile.xIndex = i;
                tile.yIndex = j;

                tiles[i, j] = tile;
                if (j == height - 1)
                {
                    tile.gameObject.AddComponent<GenerationComponent>();
                }

                
                var itemType = (GameItemType) Random.Range(1, 5);

                if (j == 5)
                {
                    itemType = GameItemType.Duck;
                }
                
                var randomItemPrefab = _prefabLibrary.GetGameItemPrefabFromType(itemType);
                var item = Instantiate(randomItemPrefab, parent: transform);
                
                tile.AttachGameItemToTile(item);   
            }
            _tileMatrix.SetTiles(tiles);
        }
        
        // EventManager.LevelStarted(levelRepresentation);
    }

    private void MoveSpentEventHandler(object sender, EventArgs args)
    {
        _moveCount--;

        if (_moveCount == 0)
        {
            EventManager.LevelFailed();
        }
    }
}

[Serializable]
public class LevelRepresentation
{
    public int gridWidth;
    public int gridHeight;
    public int moveCount;
    public string[] grid;
}
