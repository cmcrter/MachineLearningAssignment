////////////////////////////////////////////////////////////
// File: PipeEndTrigger.cs
// Author: Charles Carter
// Date Created: 08/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 08/03/22
// Brief: A script to let the manage know when a pipe has reached the end
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class PipeEndTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private WorldMover manager;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
         if(other.TryGetComponent<PipeSet>(out PipeSet pipe))
        {
            manager.PipeHasReachedTheEnd(pipe);
        }
    }

    #endregion
}
