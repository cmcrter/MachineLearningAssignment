////////////////////////////////////////////////////////////
// File: CharacterActions.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: All the possible actions for a character
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class CharacterActions : MonoBehaviour, IControllable
{
    #region Interface Contracts

    void IControllable.MoveForward() => MoveForward();
    void IControllable.MoveBackward() => MoveBackward();
    void IControllable.MoveLeft() => MoveLeft();
    void IControllable.MoveRight() => MoveRight();
    void IControllable.TurnLeft() => TurnLeft();
    void IControllable.TurnRight() => TurnRight();
    void IControllable.UseEquip() => Shoot();
    void IControllable.ThrowEquip() => Drop();

    #endregion

    #region Variables

    [SerializeField]
    private CharacterController movementController;
    [SerializeField]
    private CharacterPickupTrigger characterTrigger;
    [SerializeField]
    private CharacterHealth health;

    [SerializeField]
    private float BaseRotationSpeed = 150f;

    [SerializeField]
    private float BaseMovementSpeed = 4f;

    [SerializeField]
    private float CurrentMovementSpeed;

    //I feel like there's more potential for development with 3d calculations and smoother movement
    public float gravity = -9.81f;

    [SerializeField]
    private Transform groundCheckObj;
    [SerializeField]
    private float groundCheckRadius = 0.35f;
    [SerializeField]
    LayerMask charLayer;

    public bool isGrounded;

    [SerializeField]
    private float yVel;

    #endregion

    #region Unity Methods

    private void Start()
    {
        CurrentMovementSpeed = BaseMovementSpeed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckObj.position, groundCheckRadius, ~charLayer);

        if(isGrounded)
        {
            if(yVel < 0)
            {
                yVel = -2f;
            }
        }

        ApplyGravity();
    }

    #endregion

    #region General Movements    

    private void ApplyGravity()
    {
        yVel += gravity * Time.deltaTime;
        movementController.Move(Vector3.up * yVel * Time.deltaTime);
    }

    #endregion

    #region Movement Functions

    /// <summary>
    /// All of the actions that the Characters can do
    /// </summary>
    public void TurnRight()
    {
        transform.Rotate(transform.up * BaseRotationSpeed * Time.deltaTime);
    }

    public void TurnLeft()
    {
        transform.Rotate(transform.up * -BaseRotationSpeed * Time.deltaTime);
    }

    public void MoveForward()
    {
        movementController.Move(transform.forward * CurrentMovementSpeed * Time.deltaTime);
    }

    public void MoveBackward()
    {
        movementController.Move(-transform.forward * CurrentMovementSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        movementController.Move(-transform.right * CurrentMovementSpeed * Time.deltaTime);
    }

    public void MoveRight()
    {
        movementController.Move(transform.right * CurrentMovementSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        //If there's a gun equipped, shoot it
        characterTrigger.UseEquippable();
    }

    public void Drop()
    {
        characterTrigger.ThrowEquippable();
    }

    #endregion
}
