////////////////////////////////////////////////////////////
// File: IDamageable.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: An interface for everything with health
//////////////////////////////////////////////////////////// 

using UnityEngine;

public interface IDamageable
{
    //The necessary variables for anything with the interface
    GameObject gameObject
    {
        get;
    }

    bool canDamage
    {
        get;
    }

    public float MaxHealth
    {
        set;
        get;
    }

    public float Health
    {
        set;
        get;
    }
    

    //Technically this could be 1 function however it's less readable when you see a damage for - amount
    public void Damaged(float damage);
    public void Healed(float damage);
}
