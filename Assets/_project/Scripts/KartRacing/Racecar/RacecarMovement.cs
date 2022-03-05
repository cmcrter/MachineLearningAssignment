////////////////////////////////////////////////////////////
// File: RacecarMovement.cs
// Author: Charles Carter
// Date Created: 05/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 05/02/22
// Brief: A script to contain the actions the car can do, and runs physics for them at the correct time
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class RacecarMovement : MonoBehaviour, IControllable
{
    #region Interface Contracts

    void IControllable.MoveForward() => forward = true;
    void IControllable.MoveBackward() => brake = true;
    void IControllable.MoveLeft() => NothingAction();
    void IControllable.MoveRight() => NothingAction();
    void IControllable.TurnLeft() => steerL = true;
    void IControllable.TurnRight() => steerR = true;
    void IControllable.UseEquip() => NothingAction();
    void IControllable.ThrowEquip() => NothingAction();

    #endregion

    #region Variables

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float AccelerationSpeed = 1f;
    [SerializeField]
    private float BrakeSpeed = 1f;
    [SerializeField]
    private float steeringSpeed = 1f;

    private bool forward = false;
    private bool steerL = false;
    private bool steerR = false;
    private bool brake = false;

    private float forceModifier = 1000f;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = rb ?? GetComponent<Rigidbody>();
    }

    void Start()
    {
	
    }
 
    void FixedUpdate()
    {
        if(brake)
        {
            Brake();
        }

        if(forward)
        {
            Accerlerate();
        }

        if(steerL)
        {
            SteerLeft();
        }

        if(steerR)
        {
            SteerRight();
        }
    }

    #endregion

    #region Public Methods

    public void Brake()
    {
        rb.AddForce(-rb.transform.forward * BrakeSpeed * forceModifier, ForceMode.Force);
        brake = false;
    }

    public void Accerlerate()
    {
        rb.AddForce(rb.transform.forward * AccelerationSpeed * forceModifier, ForceMode.Force);
        forward = false;
    }

    public void SteerLeft()
    {
        Quaternion deltaRotation = Quaternion.Euler(transform.up * -steeringSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        steerL = false;
    }

    public void SteerRight()
    {
        Quaternion deltaRotation = Quaternion.Euler(transform.up * steeringSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        steerR = false;
    }

    //For actions which are not applicable to the scenario
    private void NothingAction()
    {

    }

    #endregion
}
