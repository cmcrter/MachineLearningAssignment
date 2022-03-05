////////////////////////////////////////////////////////////
// File: ExplosiveBarrel.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: A barrel that takes a few hits before exploding
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    #region Interface Contracts

    GameObject IDamageable.gameObject => gameObject;
    bool IDamageable.canDamage => gameObject.activeInHierarchy;

    float IDamageable.MaxHealth
    {
        get => hitThreshold;
        set => hitThreshold = Mathf.FloorToInt(value);
    }
    float IDamageable.Health
    {
        get => hitCounter;
        set => hitCounter = Mathf.FloorToInt(value);
    }

    void IDamageable.Damaged(float damage) => BarrellHit();
    void IDamageable.Healed(float damage) => BarrellHealed();

    #endregion

    #region Variables

    //The barrell will take a certain amount of hits before exploding, less about individual damage numbers
    [SerializeField]
    private int hitCounter;

    [SerializeField]
    private int hitThreshold = 3;
    [SerializeField]
    private float explosionRadius = 2f;
    [SerializeField]
    private float explosionDamage = 25f;

    [SerializeField]
    private LayerMask hitMask;

    #endregion

    #region Unity Methods

    void Start()
    {
        hitCounter = 0;
    }
 
    #endregion

    #region Private Methods

    private void BarrellHit()
    {
        hitCounter++;
        Mathf.Clamp(hitCounter, 0, hitThreshold);

        if(CheckForBarrellDeath())
        {
            DeathEffect();
        }
    }

    private void BarrellHealed()
    {
        return;
    }

    private bool CheckForBarrellDeath()
    {
        if(hitCounter == hitThreshold)
        {
            return true;
        }

        return false;
    }

    private void DeathEffect()
    {
        //Getting all the relevant colliders to then damage
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius, hitMask);

        for(int i = 0; i < hitObjects.Length; ++i)
        {
            if(hitObjects[i].TryGetComponent(out IDamageable damageable))
            {
                //The further away, the less damage from the explosive (directly proportional)
                float modifiedDamage = explosionDamage * Mathf.Clamp01(1 - Vector3.Distance(transform.position, hitObjects[i].transform.position) / explosionRadius);  
                damageable.Damaged(modifiedDamage);
            }
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

    #endregion
}
