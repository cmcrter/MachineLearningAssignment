////////////////////////////////////////////////////////////
// File: LoadingScreen.cs
// Author: Charles Carter
// Date Created: 16/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/05/22
// Brief: A script to handle loading between scenes
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [System.Serializable]
    internal class SceneLoadingInfo
    {
        public string Title;
        public string Description;
        public Sprite SceneSprite;
    }

    #region Variables

    [Header("Loading Title Effect")]
    [SerializeField]
    private TextMeshProUGUI loadingText;

    [SerializeField]
    float fadeDuration = 5f;
    Coroutine fadeCoroutine;

    [Header("Loading Screen Scene Info Objects")]
    [SerializeField]
    private GameObject sceneInfoObject;

    [SerializeField]
    private TextMeshProUGUI sceneTitleText;
    [SerializeField]
    private Image sceneImage;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    //There may be a cleaner way to do this, not sure. Probably some public static array with scriptable objects
    [SerializeField]
    private List<SceneLoadingInfo> info = new List<SceneLoadingInfo>();

    //Being able to override for testing purposes
    [Header("Override Loading Screen Information")]
    [SerializeField]
    private bool bOverride;
    [SerializeField]
    private bool bGoToNextScene;
    [SerializeField]
    private TypeOfScene infoToUse;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(bOverride)
        {
            LoadingInformation.scene = infoToUse;
        }

        //Based on LoadingInformation change text, image etc
        switch(LoadingInformation.scene)
        {
            case TypeOfScene.TicTacToe:
                ShowSceneInfo(0);
                break;
            case TypeOfScene.FlappyMusic:
                ShowSceneInfo(1);
                break;
            case TypeOfScene.KartRacing:
                ShowSceneInfo(2);
                break;
            case TypeOfScene.Hummingbird:
                ShowSceneInfo(3);
                break;
            case TypeOfScene.Shooter:
                ShowSceneInfo(4);
                break;
            //Intentional fall through
            case TypeOfScene.MainMenu:
            case TypeOfScene.UNKNOWN:
            default:
                sceneInfoObject.SetActive(false);
                break;
        }
    }

    void Start()
    {
        StartCoroutine(LoadYourAsyncScene());
        fadeCoroutine = StartCoroutine(Co_FadeText());
    }

    #endregion

    #region Private Methods

    private void ShowSceneInfo(int infoIndex)
    {
        sceneTitleText.text = info[infoIndex].Title;
        descriptionText.text = info[infoIndex].Description;

        sceneImage.sprite = info[infoIndex].SceneSprite;
    }

    private IEnumerator Co_FadeText()
    {
        //Using the built in fade in and out functions
        loadingText.CrossFadeAlpha(0.0f, fadeDuration, false);

        yield return new WaitForSeconds(fadeDuration);

        // and back over 500ms.
        loadingText.CrossFadeAlpha(1.0f, fadeDuration, false);

        yield return new WaitForSeconds(fadeDuration);

        //Forever looping
        fadeCoroutine = null;
        fadeCoroutine = StartCoroutine(Co_FadeText());
    }

    private IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadingInformation.SceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while(!asyncLoad.isDone)
        {
            //The level is ready to finish loading
            if(asyncLoad.progress >= 0.9f)
            {
                if(!bOverride || (bOverride && bGoToNextScene))
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    #endregion
}
