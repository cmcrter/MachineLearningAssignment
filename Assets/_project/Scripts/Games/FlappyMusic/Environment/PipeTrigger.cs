////////////////////////////////////////////////////////////
// File: PipeTrigger.cs
// Author: Charles Carter
// Date Created: 08/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 08/03/22
// Brief: A script for the pipes to tell the instance manager when a player has gotten a point
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class PipeTrigger : MonoBehaviour
{
    #region Variables

    public WorldMover manager;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            if(manager && enabled)
            {
                manager.AddScore();
                enabled = false;
            }
        }
    }

    #endregion
}
