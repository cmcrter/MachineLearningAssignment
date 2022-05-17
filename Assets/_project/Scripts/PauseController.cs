////////////////////////////////////////////////////////////
// File: PauseController.cs
// Author: Charles Carter
// Date Created: 17/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 17/05/22
// Brief: A script to go on the pause menu prefab so any game can pause
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class PauseController : MonoBehaviour
{
    #region Variables 

    [SerializeField]
    private GameObject pauseUI;

    float previousTimeScale = 1;

    #endregion

    #region Unity Methods
    void Start()
    {
	
    }
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }	
    }
    #endregion

    #region Public Methods


    public void TogglePause()
    {
        if(pauseUI)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);

            if(Time.timeScale != 1 && pauseUI.activeSelf)
            {
                previousTimeScale = Time.timeScale;
            }

            Time.timeScale = pauseUI.activeSelf ? 0 : previousTimeScale;
        }
    }

    public void TogglePause(bool bOn)
    {
        if(pauseUI)
        {
            pauseUI.SetActive(bOn);
            Time.timeScale = bOn ? 0 : 1;
        }
    }

    #endregion
}
