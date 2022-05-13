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
    bool IEquipable.canShoot => canFire;

    void IEquipable.Drop(Vector3 direction, float power, Transform parent) => DropGun(direction, power, parent);
    void IEquipable.Pickup(Transform handPos, MLShooter shooter) => PickupGun(handPos, shooter);

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

    [SerializeField]
    private Transform barrellPos;

    [SerializeField]
    ShooterInstanceManager instanceManager;

    [SerializeField]
    private float shotCooldownDuration = 0.3f;

    private MLShooter holder;

    private Vector3 startPos;
    private Quaternion startRot;

    [SerializeField]
    int gunLayer;

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void Start()
    {
        instanceManager = ShooterInstanceManager.instance;
    }

    private void PickupGun(Transform handPos, MLShooter shooter)
    {
        gameObject.layer = shooter.gameObject.layer;

        bPickupable = false;
        rb.isKinematic = true;

        transform.position = handPos.position;
        transform.rotation = handPos.rotation;
        transform.SetParent(handPos);

        holder = shooter;
        holder.AddReward(0.25f);
    }

    private void DropGun(Vector3 direction, float power, Transform newParent)
    {
        transform.SetParent(newParent);
        gameObject.layer = gunLayer;
        rb.isKinematic = false;

        rb.AddRelativeForce(direction * power, ForceMode.Impulse);

        StartCoroutine(Co_PickUpCooldown());

        holder = null;
    }

    private void FireGun()
    {
        if(!bFireable)
        {
            return;
        }

        GameObject bulletToUse = instanceManager.bulletPool.Get();

        //So the bullet doesn't collide with the gun/player when it turns on
        bulletToUse.layer = gameObject.layer;

        //Moving the bullet to the right place
        bulletToUse.transform.position = barrellPos.transform.position;

        //Firing the bullet out
        if(bulletToUse.TryGetComponent(out Bullet bullet))
        {
            bullet.Fired(barrellPos.transform.forward, 45f, holder);
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

    public void ResetGun()
    {
        transform.SetParent(null);
        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.layer = gunLayer;
        rb.isKinematic = false;
    }

    #endregion
}
