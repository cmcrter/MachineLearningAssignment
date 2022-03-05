////////////////////////////////////////////////////////////
// File: PlayerDriver.cs
// Author: Charles Carter
// Date Created: 05/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 05/03/22
// Brief: The player's controller script to drive
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class PlayerDriver : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private RacecarMovement actions;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        actions = actions ?? GetComponent<RacecarMovement>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            actions.SteerLeft();
        }

        if(Input.GetKey(KeyCode.S))
        {
            actions.Brake();
        }

        if(Input.GetKey(KeyCode.W))
        {
            actions.Accerlerate();
        }

        if(Input.GetKey(KeyCode.D))
        {
            actions.SteerRight();
        }
    }

    #endregion
 
    #region Private Methods
    #endregion
}
