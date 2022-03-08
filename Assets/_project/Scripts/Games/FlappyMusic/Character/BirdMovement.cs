////////////////////////////////////////////////////////////
// File: BirdMovement.cs
// Author: Charles Carter
// Date Created: 08/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 08/03/22
// Brief: The script to control the rigidbody of the bird
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private WorldMover manager;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float tapforce = 15f;
    [SerializeField]
    private float turnSpeed = 5f;
    
    [SerializeField]
    private bool bPlayerControlled = false;

    private Vector3 intitialPos;
    
    #endregion
 
    #region Unity Methods

    private void Awake()
    {
        rb = rb ?? GetComponent<Rigidbody>();
    }

    private void Start()
    {
        intitialPos = transform.position;
    }

    //Simple enough to just be in this script
    private void Update()
    {
        //Rotating the player based on the velocity it's moving, so it's always facing towards where it's going
        if(rb.velocity.magnitude > 0.1)
        {
            Vector3 newVector = rb.velocity + new Vector3(0, 0, 10f);

            Quaternion dirQ = Quaternion.LookRotation(newVector);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, rb.velocity.magnitude * turnSpeed * Time.deltaTime);
            rb.MoveRotation(slerp);
        }

        //If this is an ML agent, there's nothing to be done. I do realise that with Unity ML I could edit the Heuristic
        // although it wouldn't be as easy if I wanted to use other ML packages too.
        if(!bPlayerControlled)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Tap();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Call this instead of player died so there's no punishment
            manager.GameComplete();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer != 7)
        {
            //Player Died
            rb.isKinematic = true;
            manager.PlayerDied();
        }
    }

    #endregion

    #region Public Methods

    public void Tap()
    {
        if(!rb)
            return;

        rb.AddForce(Vector3.up * tapforce, ForceMode.Impulse);
    }

    public virtual void ResetPlayer()
    {
        transform.rotation = Quaternion.identity;
        transform.position = intitialPos;
        rb.isKinematic = false;
    }

    public float GetYVel()
    {
        return rb.velocity.y;
    }

    #endregion
}
