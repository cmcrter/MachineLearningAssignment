////////////////////////////////////////////////////////////
// File: CheckpointTrigger.cs
// Author: Charles Carter
// Date Created: 21/03/21
// Last Edited By: Charles Carter
// Date Last Edited: 21/03/21
// Brief: The checkpoints' trigger to tell the instance manager when a car has entered it
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private RaceManager manager;
    [SerializeField]
    private int checkpointID;

    #endregion
 
    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerDriver driver))
        {
            manager.DriverCrossedCheckpoint(driver, checkpointID);
        }
        else if(other.TryGetComponent(out MLDriver agent))
        {
            manager.DriverCrossedCheckpoint(agent, checkpointID);
        }
    }

    #endregion
}
