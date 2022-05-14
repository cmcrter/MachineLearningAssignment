////////////////////////////////////////////////////////////
// File: KRDFinishLineTrigger.cs
// Author: Charles Carter
// Date Created: 14/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 14/05/22
// Brief: A script to tell the manager when a car is finished
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class KRDFinishLineTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private KartDemoManager manager;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other) 
    {  
        if(other.TryGetComponent(out MLDriver agent))
        {
            manager.DriverCrossedFinish(agent);
        }
    }

    #endregion
}
