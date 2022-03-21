////////////////////////////////////////////////////////////
// File: MLDriver.cs
// Author: Charles Carter
// Date Created: 21/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/03/22
// Brief: The driver controlled by Unity ML
//////////////////////////////////////////////////////////// 

using UnityEngine;
using Unity.MLAgents;

public class MLDriver : Agent /*, IAgentable*/
{
    #region Variables

    public int driverID = 0;

    public int checkpointNum = 0;
    public int lapCount = 0;

    #endregion
 
    #region Unity Methods

    void Start()
    {
	
    }
 
    void Update()
    {
	
    }

    #endregion
 
    #region Private Methods
    #endregion
}
