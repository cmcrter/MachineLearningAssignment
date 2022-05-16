////////////////////////////////////////////////////////////
// File: MainMenu.cs
// Author: Charles Carter
// Date Created: 16/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/05/22
// Brief: Buttons etc for main menu
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Variables

    [Header("Base Panels")]
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private GameObject PlayPanel;

    #endregion
 
    #region Unity Methods

    void Start()
    {
	
    }
 
    #endregion

    #region Public Methods

    //Simple buttons, since playpanel overlaps the normal panel
    public void OpenPlayPanel()
    {
        PlayPanel.SetActive(true);
    }

    public void Back()
    {
        PlayPanel.SetActive(false);
    }

    #endregion

    #region Private Methods
    #endregion
}
