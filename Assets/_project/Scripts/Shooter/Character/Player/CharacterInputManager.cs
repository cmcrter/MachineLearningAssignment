////////////////////////////////////////////////////////////
// File: CharacterInputManager.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: An script for a player to control a Character
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class CharacterInputManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private CharacterActions actions;

    #endregion

    #region Unity Methods

    private void Update()
    {
        if(actions)
        {
            if(actions.isGrounded)
            {
                GroundedMovement();
            }
        }
    }

    #endregion

    #region Private Methods

    private void GroundedMovement()
    {
        //The reference to the actions are necessary 
        if(!actions)
            return;

        if(Input.GetKey(KeyCode.A))
        {
            
        }

        if(Input.GetKey(KeyCode.S))
        {
            
        }

        if(Input.GetKey(KeyCode.W))
        {

        }

        if(Input.GetKey(KeyCode.D))
        {

        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {

        }

        if(Input.GetKey(KeyCode.RightArrow))
        {

        }
    }

    #endregion
}