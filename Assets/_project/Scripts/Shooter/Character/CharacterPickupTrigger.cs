////////////////////////////////////////////////////////////
// File: CharacterPickupTrigger.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: A script to register when the character is over an equippable, to auto-equip
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class CharacterPickupTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Transform handTransform;

    [SerializeField]
    private IEquipable currentlyEquipped;

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

                equippable.Pickup(handTransform);

                currentlyEquipped = equippable;
            }
        }
    }

    #endregion

    #region Private Methods

    private void UseEquippable()
    {
        currentlyEquipped.UseEquippable();
    }

    private void ThrowEquippable()
    {
        currentlyEquipped.Drop(handTransform.forward, 4f);

        currentlyEquipped = null;
    }

    #endregion
}
