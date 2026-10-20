using System.Collections.Generic;

public static class Constants
{
    public const int MinConnectedCubeCountForLegalMove = 1;
    public const int MinConnectedCubeCountForRocketCreation = 333;
    public const float GameItemOneTileFallDurationInSeconds = 0.1f;

    public const string PLayerPrefsLevelKey = "PlayerLevel";
    
    public const string LevelSceneName = "LevelScene";
    public const string MainSceneName = "MainScene";

    
    public enum GameItemType
    {
        None = 0,
        
        // Cubes
        BlueCube = 1,
        GreenCube = 2,
        RedCube = 3,
        PurpleCube = 6,
        YellowCube = 4,
        RandomCube = 5,

        // SpecialItems
        Rocket = 101,
        
        // Obstacles
        Balloon = 1001,
        Duck = 1002,
    }
    
    public static readonly List<GameItemType> AllCubeTypes = new ()
    {
        GameItemType.BlueCube,
        GameItemType.GreenCube,
        GameItemType.PurpleCube,
        GameItemType.RedCube,
        GameItemType.YellowCube,
    };
}
