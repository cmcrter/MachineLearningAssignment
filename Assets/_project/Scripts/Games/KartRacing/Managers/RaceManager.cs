////////////////////////////////////////////////////////////
// File: RaceManager.cs
// Author: Charles Carter
// Date Created: 21/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/03/22
// Brief: A manager to control this race instance
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private List<MLDriver> MLDrivers = new List<MLDriver>();
    [SerializeField]
    private List<CheckpointTrigger> checkpoints = new List<CheckpointTrigger>();

    [SerializeField]
    private List<Transform> startingPoints = new List<Transform>();

    #endregion

    #region Unity Methods

    private void Awake()
    {
        ResetCars();
    }

    void Start()
    {
	
    }
 
    void Update()
    {
	
    }
    #endregion

    #region Public Methods

    public void ResetCars()
    {
        for(int i = 0; i < MLDrivers.Count; ++i)
        {
            MLDrivers[i].driverID = i;
            MLDrivers[i].lapCount = 0;
            MLDrivers[i].checkpointNum = 0;

            if(startingPoints[i])
            {
                MLDrivers[i].transform.position = new Vector3(startingPoints[i].position.x, 0, startingPoints[i].position.z);
                MLDrivers[i].transform.rotation = startingPoints[i].rotation;
            }
        }
    }

    public void DriverCrossedCheckpoint(MLDriver driver, int CheckpointID)
    {
        if(MLDrivers[driver.driverID].checkpointNum == CheckpointID - 1)
        {
            MLDrivers[driver.driverID].checkpointNum = CheckpointID;
        }
    }

    public void DriverCrossedFinishLine(MLDriver driver)
    {
        if(MLDrivers[driver.driverID].checkpointNum == checkpoints.Count)
        {
            MLDrivers[driver.driverID].lapCount++;
            MLDrivers[driver.driverID].checkpointNum = 0;
        }
    }

    public void DriverCrossedCheckpoint(PlayerDriver driver, int CheckpointID)
    {

    }

    public void DriverCrossedFinishLine(PlayerDriver driver)
    {

    }

    #endregion
}
