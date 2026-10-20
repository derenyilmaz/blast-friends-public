
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Constants;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TouchController))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(FallManager))]
public class Board : MonoBehaviour
{
    private TileMatrix _tileMatrix;
    
    private TouchController _touchController;
    private LevelManager _levelManager;
    private FallManager _fallManager;

    private PrefabLibrary _prefabLibrary;
    
    private void Awake()
    {
        _touchController = GetComponent<TouchController>();
        _levelManager = GetComponent<LevelManager>();
        _fallManager = GetComponent<FallManager>();
        
        _tileMatrix = new TileMatrix();
        
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
    }

    private void Start()
    {
        EventManager.OnCubeTapped += CubeTappedEventHandler;
        EventManager.OnRocketTapped += RocketTappedEventHandler;
        
        _levelManager.Configure(_tileMatrix);
        _fallManager.Configure(_tileMatrix);
    }
    

    private void OnDestroy()
    {
        EventManager.OnCubeTapped -= CubeTappedEventHandler;
        EventManager.OnRocketTapped -= RocketTappedEventHandler;
    }


    private void CubeTappedEventHandler(object sender, EventManager.CubeTappedEventArgs eventArgs)
    {
        var tappedCube = eventArgs.Cube;
        
        if (tappedCube == null)
        {
            return;
        }

        var startTile = _tileMatrix[tappedCube.xIndex, tappedCube.yIndex];
        var predicate = new Func<Tile, bool>(tile => tile.GetGameItem() != null && 
                                                     tile.GetGameItem().gameItemType == tappedCube.gameItemType);
        
        var connectedTilesWithSameCubeType = _tileMatrix.GetConnectedTilesWithPredicate(startTile, predicate);
        
        if (connectedTilesWithSameCubeType.Count < MinConnectedCubeCountForLegalMove)
        {
            return;
        }

        EventManager.MoveSpent();
        
        var boundaryTiles = _tileMatrix.GetBoundaryTiles(connectedTilesWithSameCubeType);
        foreach (var tile in boundaryTiles)
        {
            tile.GetGameItem()?.TakeDamageFromBoundary();
        }

        if (connectedTilesWithSameCubeType.Count < MinConnectedCubeCountForRocketCreation)
        {
            RegularMove(connectedTilesWithSameCubeType);
            return;
        }
        
        RocketCreatingMove(connectedTilesWithSameCubeType, startTile);
    }

    private void RegularMove(HashSet<Tile> tileList)
    {
        foreach (var tile in tileList)
        {
            var connectedCube = tile.GetGameItem();
            if (connectedCube == null)
            {
                continue;
            }
            
            connectedCube.Break();
        }
    }

    private void RocketCreatingMove(HashSet<Tile> tileList, Tile startTile)
    {
        foreach (var tile in tileList)
        {
            var connectedCube = tile.GetGameItem();
            if (connectedCube == null)
            {
                continue;
            }
            
            connectedCube.Break();
            tile.PurgeGameItem();
        }

        var rocketPrefab = _prefabLibrary.GetGameItemPrefabFromType(GameItemType.Rocket);

        var rocket = Instantiate(rocketPrefab) as Rocket;

        if (rocket == null)
        {
            return;
        }
        
        startTile.AttachGameItemToTile(rocket);
    }

    private void RocketTappedEventHandler(object sender, EventManager.RocketTappedEventArgs args)
    {
        var tappedRocket = args.Rocket;
        tappedRocket.Explode();
        EventManager.MoveSpent();
    }
}                                       
