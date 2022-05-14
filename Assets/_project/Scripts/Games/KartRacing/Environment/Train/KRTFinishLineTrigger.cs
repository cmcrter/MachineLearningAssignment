////////////////////////////////////////////////////////////
// File: KRTFinishLineTrigger.cs
// Author: Charles Carter
// Date Created: 05/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 05/03/22
// Brief: The trigger to tell the karting game manager that a car has crossed the finish line (and making sure it's a valid lap)
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private RaceManager manager;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerDriver driver))
        {
            manager.DriverCrossedFinishLine(driver);
        }
        else if(other.TryGetComponent(out MLDriver agent))
        {
            manager.DriverCrossedFinishLine(agent);
        }
    }

    #endregion
}
