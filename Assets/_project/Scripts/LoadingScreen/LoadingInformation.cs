////////////////////////////////////////////////////////////
// File: LoadingInformation.cs
// Author: Charles Carter
// Date Created: 16/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/05/22
// Brief: A class to hold data between scenes, especially the loading screen
//////////////////////////////////////////////////////////// 

public enum TypeOfScene
{
    MainMenu,
    TicTacToe,
    FlappyShip,
    KartRacing,
    Hummingbird,
    Shooter,
    UNKNOWN,
    COUNT
}

public static class LoadingInformation
{
    public static string SceneToLoad;
    public static TypeOfScene scene;
}
