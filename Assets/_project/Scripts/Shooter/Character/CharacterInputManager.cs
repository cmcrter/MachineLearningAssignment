////////////////////////////////////////////////////////////
// File: CharacterManager.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: A script to control a Character (Where the inputs are handled)
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.Pool;

namespace Shooter
{
    public class CharacterInputManager : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private CharacterController movementController;
        [SerializeField]
        private CharacterPickupTrigger characterTrigger;
        [SerializeField]
        private CharacterHealth health;

        [SerializeField]
        private float BaseRotationSpeed = 1.5f;

        [SerializeField]
        private float BaseMovementSpeed = 4f;

        [SerializeField]
        private float CurrentMovementSpeed;

        //I feel like there's more potential for development with 3d calculations and smoother movement
        [SerializeField]
        private float gravity = -9.81f;

        [SerializeField]
        private Transform groundCheckObj;
        [SerializeField]
        private float groundCheckRadius = 0.35f;
        [SerializeField]
        LayerMask charLayer;
        [SerializeField]
        private bool isGrounded;

        [SerializeField]
        private float yVel;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            
        }

        private void Start()
        {
            CurrentMovementSpeed = BaseMovementSpeed;
        }

        private void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheckObj.position, groundCheckRadius, ~charLayer);

            if(isGrounded)
            {
                GroundedMovement();
            }
            else
            {
                AirMovement();
            }

            if(health)
            {
                if(Input.GetKeyDown(KeyCode.Delete))
                {
                    IDamageable damageDealer = health;
                    damageDealer.Damaged(15f);
                }
            }

            if(characterTrigger && characterTrigger.bEquipped)
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    //If there's a gun equipped, shoot it
                    characterTrigger.UseEquippable();
                }

                if(Input.GetKey(KeyCode.Q))
                {
                    characterTrigger.ThrowEquippable();
                }
            }

            //Applying gravity
            yVel += gravity * Time.deltaTime;
            movementController.Move(Vector3.up * yVel * Time.deltaTime);
        }
        #endregion

        #region Private Methods

        private void GroundedMovement()
        {
            if(yVel < 0)
            {
                yVel = -2f;
            }

            if(!movementController)
                return;

            if(Input.GetKey(KeyCode.A))
            {
                movementController.Move(-transform.right * CurrentMovementSpeed * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.S))
            {
                movementController.Move(-transform.forward * CurrentMovementSpeed * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.W))
            {
                movementController.Move(transform.forward * CurrentMovementSpeed * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.D))
            {
                movementController.Move(transform.right * CurrentMovementSpeed * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(transform.up * -BaseRotationSpeed * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(transform.up * BaseRotationSpeed * Time.deltaTime);
            }
        }

        private void AirMovement()
        {

        }

        #endregion
    }
}
