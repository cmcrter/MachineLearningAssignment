////////////////////////////////////////////////////////////
// File: CharacterPickupTrigger.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: A script to register when the character is over an equippable, to auto-equip
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;

public class CharacterPickupTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Transform handTransform;
    private IEquipable currentlyEquipped;

    [SerializeField]
    private Transform equipObj;
    [SerializeField]
    private Transform equipParent;

    public bool bEquipped = false;

    [SerializeField]
    MLShooter shooter;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IEquipable equippable))
        {
            if(equippable.canPickup && handTransform)
            {
                //Putting it in the right spot
                //equippable.transform.position = handTransform.position;
                //equippable.transform.rotation = handTransform.rotation;

                if(currentlyEquipped != null)
                {
                    ThrowEquippable();
                }

                equippable.Pickup(handTransform, gameObject.layer, shooter);

                currentlyEquipped = equippable;
                equipObj = equippable.transform;
                bEquipped = true;

                shooter.AddReward(1f);
            }
        }
    }

    #endregion

    #region Public Methods

    public void UseEquippable()
    {
        if(currentlyEquipped == null)
        {
            return;
        }

        currentlyEquipped.UseEquippable();
    }

    public void ThrowEquippable()
    {
        if(currentlyEquipped == null) 
        {
            return;
        }

        currentlyEquipped.Drop(handTransform.forward, 4f, equipParent);

        currentlyEquipped = null;
        equipObj = null;
        bEquipped = false;

        shooter.AddReward(-3f);
    }

    #endregion

    #region Private Methods

    #endregion
}
