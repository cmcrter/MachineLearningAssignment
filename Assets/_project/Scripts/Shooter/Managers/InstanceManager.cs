////////////////////////////////////////////////////////////
// File: InstanceManager.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: A script for AI to train in a simulated game
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;

//Not a singleton due to multiple instances speeding up training
public class InstanceManager : MonoBehaviour
{
    public enum TrainingType
    {
        vsPlayer = 0,
        AimPractice = 1,
        vsSelf
    }

    #region Variables

    [SerializeField]
    private TrainingType currentTraining;

    [SerializeField]
    private List<IAgentable> Agents;

    #endregion

    #region Unity Methods
    #endregion

    #region Private Methods
    #endregion
}
