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
        SceneManager.LoadScene("LoadingScreen");
    }

    public static void MainMenu()
    {
        LoadingInformation.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }
}
