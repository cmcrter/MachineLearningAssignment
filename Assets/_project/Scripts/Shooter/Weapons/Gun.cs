////////////////////////////////////////////////////////////
// File: Gun.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: A base script for all guns to inherit from
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour, IEquipable
{
    #region Interface Contracts

    Transform IEquipable.transform => transform;
    bool IEquipable.canPickup => bPickupable;

    void IEquipable.Drop(Vector3 direction, float power, Transform parent) => DropGun(direction, power, parent);
    void IEquipable.Pickup(Transform handPos, int LayerToGoTo) => PickupGun(handPos, LayerToGoTo);

    void IEquipable.UseEquippable() => FireGun();

    #endregion

    #region Variables

    private bool bPickupable = true;

    public bool canFire
    {
        get => bFireable;
        set => bFireable = value;
    }
    private bool bFireable = true;

    [SerializeField]
    private Rigidbody rb;

    IObjectPool<GameObject> currentbulletpool;

    [SerializeField]
    private Transform barrellPos;

    [SerializeField]
    InstanceManager instanceManager;

    [SerializeField]
    private float shotCooldownDuration = 0.3f;

    #endregion

    #region Public Methods

    public void InstialiseGun(IObjectPool<GameObject> bulletpool)
    {
        currentbulletpool = bulletpool;
    }

    #endregion

    #region Private Methods

    private void PickupGun(Transform handPos, int layer)
    {
        gameObject.layer = layer;

        bPickupable = false;
        rb.isKinematic = true;

        transform.position = handPos.position;
        transform.rotation = handPos.rotation;
        transform.SetParent(handPos);
    }

    private void DropGun(Vector3 direction, float power, Transform newParent)
    {
        transform.SetParent(newParent);
        gameObject.layer = 0;
        rb.isKinematic = false;

        rb.AddRelativeForce(direction * power, ForceMode.Impulse);

        StartCoroutine(Co_PickUpCooldown());
    }

    private void FireGun()
    {
        if(!bFireable)
        {
            return;
        }

        if(currentbulletpool == null)
        {
            currentbulletpool = instanceManager.bulletPool;
        }

        GameObject bulletToUse = currentbulletpool.Get();

        //So the bullet doesn't collide with the gun/player when it turns on
        bulletToUse.layer = gameObject.layer;

        //Moving the bullet to the right place
        bulletToUse.transform.position = barrellPos.transform.position;

        //Firing the bullet out
        if(bulletToUse.TryGetComponent(out Bullet bullet))
        {
            bullet.Fired(barrellPos.transform.forward, 45f);
        }

        StartCoroutine(Co_ShootCooldown());
    }

    private IEnumerator Co_PickUpCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        bPickupable = true;
    }

    public IEnumerator Co_ShootCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(shotCooldownDuration);
        canFire = true;
    }

    #endregion
}
