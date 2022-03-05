////////////////////////////////////////////////////////////
// File: CharacterInputManager.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: An script for a player to control a Character
//////////////////////////////////////////////////////////// 

using UnityEngine;

[RequireComponent(typeof(CharacterActions))]
public class CharacterInputManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private CharacterActions actions;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        actions = actions ?? GetComponent<CharacterActions>();
    }

    private void Update()
    {
        if(actions)
        {
            if(actions.isGrounded)
            {
                GroundedMovement();
                WeaponControl();
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
            actions.MoveLeft();
        }

        if(Input.GetKey(KeyCode.S))
        {
            actions.MoveBackward();
        }

        if(Input.GetKey(KeyCode.W))
        {
            actions.MoveForward();
        }

        if(Input.GetKey(KeyCode.D))
        {
            actions.MoveRight();
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            actions.TurnLeft();
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            actions.TurnRight();
        }
    }

    private void WeaponControl()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            actions.Shoot();
        }

        if(Input.GetKey(KeyCode.Q))
        {
            actions.Drop();
        }
    }

    #endregion
}