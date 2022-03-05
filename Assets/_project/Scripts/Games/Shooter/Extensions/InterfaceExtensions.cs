////////////////////////////////////////////////////////////
// File: InterfaceExtensions.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: A static class to implement extension functions across all the interfaces
//////////////////////////////////////////////////////////// 

using UnityEngine;

public static class InterfaceExtensions
{
    //These were ideas, although unused it's an interesting idea
    public static GameObject ReturnGameObject(this IDamageable interfaceUsed, MonoBehaviour behaviour)
    {
        return behaviour.gameObject;
    }

    public static Transform ReturnTransform(this IDamageable interfaceUsed, MonoBehaviour behaviour)
    {
        return behaviour.transform;
    }
}
