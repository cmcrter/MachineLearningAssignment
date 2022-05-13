////////////////////////////////////////////////////////////
// File: Bullet.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited:  22/02/22
// Brief: Damages whatever it hits
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private ShooterInstanceManager instanceManager;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float Damage;

    [SerializeField]
    private MLShooter shooterOfBullet;

    #endregion
 
    #region Unity Methods
    void Start()
    {
	
    }

    private void OnEnable()
    {
        instanceManager = ShooterInstanceManager.instance;
    }

    #endregion

    #region Public Methods

    public Rigidbody GetRB()
    {
        return rb;
    }

    public void Fired(Vector3 direction, float force, MLShooter shooter)
    {
        gameObject.layer = shooter.gameObject.layer;

        shooterOfBullet = shooter;
        instanceManager = shooterOfBullet.instanceManager;

        gameObject.SetActive(true);

        rb.isKinematic = false;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    #endregion

    #region Private Methods

    private void OnDisable()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            if(damageable.canDamage)
            {
                damageable.Damaged(Damage);
                instanceManager.RewardShooter(shooterOfBullet);
                shooterOfBullet.bShotSomething = true;
            }
        }

        //Debug.Log("This Hit: " + other.name + " this is the shooter: " + shooterOfBullet.name);
        instanceManager.bulletPool.Release(gameObject);
        shooterOfBullet = null;
    }

    #endregion
}
