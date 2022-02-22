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
    private InstanceManager instanceManager;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float Damage;

    #endregion
 
    #region Unity Methods
    void Start()
    {
	
    }
 
    void Update()
    {
	
    }
    #endregion

    #region Public Methods

    public void Fired(Vector3 direction, float force)
    {
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
            }
        }

        if(!instanceManager)
        {
            Destroy(gameObject);
            return;
        }

        instanceManager.bulletPool.Release(gameObject);
    }

    #endregion
}
