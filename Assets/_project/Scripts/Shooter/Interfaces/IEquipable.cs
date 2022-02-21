////////////////////////////////////////////////////////////
// File: IEquippable.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: The interface for items that can be equipped by the player
//////////////////////////////////////////////////////////// 

using UnityEngine;

public interface IEquipable
{    
    bool canPickup
    {
        get;
    }

    public void Pickup();
    public void Drop();
}
