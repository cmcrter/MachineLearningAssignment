////////////////////////////////////////////////////////////
// File: KRDCheckpointTrigger.cs
// Author: Charles Carter
// Date Created: 14/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 14/05/22
// Brief: Making sure the AI follow the track fully
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class KRDCheckpointTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private KartDemoManager manager;
    [SerializeField]
    private int checkpointID;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MLDriver agent))
        {
            manager.DriverCrossedCheckpoint(agent, checkpointID);
        }
    }

    #endregion
}
