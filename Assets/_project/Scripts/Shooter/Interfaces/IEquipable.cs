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
    Transform transform
    {
        get;
    }

    bool canPickup
    {
        get;
    }

    public void UseEquippable();
    public void Pickup(Transform handPos);

    //The drop can be used for multiple types (higher speed drop is a throw)
    public void Drop(Vector3 direction, float power);
}
