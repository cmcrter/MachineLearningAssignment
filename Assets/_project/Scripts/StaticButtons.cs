////////////////////////////////////////////////////////////
// File: StaticButtons.cs
// Author: Charles Carter
// Date Created: 16/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/05/22
// Brief: A script for general reused buttons in the project
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticButtons : MonoBehaviour
{
    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void LoadScene(string sceneName)
    {
        LoadingInformation.SceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void LoadTTTScene()
    {
        LoadingInformation.SceneToLoad = "TicTacToeDemo";
        LoadingInformation.scene = TypeOfScene.TicTacToe;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void LoadFlappyMusicScene()
    {
        LoadingInformation.SceneToLoad = "FlappyShipDemo";
        LoadingInformation.scene = TypeOfScene.FlappyShip;
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadHummingbirdScene()
    {
        LoadingInformation.SceneToLoad = "HummingbirdDemo";
        LoadingInformation.scene = TypeOfScene.Hummingbird;
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadKartRacingScene()
    {
        LoadingInformation.SceneToLoad = "KartRacingDemo";
        LoadingInformation.scene = TypeOfScene.KartRacing;
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadShooterScene()
    {
        LoadingInformation.SceneToLoad = "ShooterDemo";
        LoadingInformation.scene = TypeOfScene.Shooter;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void MainMenu()
    {
        LoadingInformation.SceneToLoad = "MainMenu";
        LoadingInformation.scene = TypeOfScene.MainMenu;
        SceneManager.LoadScene("LoadingScene");
    }
}
