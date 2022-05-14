////////////////////////////////////////////////////////////
// File: KartTrainingManager.cs
// Author: Charles Carter
// Date Created: 21/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/22
// Brief: A manager to control this race instance
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartTrainingManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private List<MLDriver> MLDrivers = new List<MLDriver>();
    [SerializeField]
    private List<KRTCheckpointTrigger> checkpoints = new List<KRTCheckpointTrigger>();

    [SerializeField]
    private List<Transform> startingPoints = new List<Transform>();

    [SerializeField]
    private int checkpointReward = 20;

    [SerializeField]
    private int lapReward = 150;

    [SerializeField]
    private KRTRaceUI UI;

    private int finishedDrivers = 0;

    [SerializeField]
    private float maxLapLength = 300f;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        ResetCars();
    }

    void Start()
    {
        foreach(MLDriver driver in MLDrivers)
        {
            driver.OnEpisodeBegin();
        }
    }
 
    void Update()
    {
        if(!UI)
        {
            return;
        }

        if(UI.ReturnCurrentTime() >= maxLapLength)
        {
            foreach(MLDriver driver in MLDrivers)
            {
                if(driver.lapCount == 0)
                {
                    driver.AddReward(-10f);
                    driver.EndEpisode();
                }
            }

            UI.StopLapTimer();
            StartCoroutine(Co_StartNextRace());
        }
        else
        {
            int playersCompleted = 0;

            foreach(MLDriver driver in MLDrivers)
            {
                if(driver.lapCount == 1)
                {
                    playersCompleted++;
                }
            }

            if(playersCompleted == MLDrivers.Count)
            {
                UI.StopLapTimer();
                StartCoroutine(Co_StartNextRace());
            }
        }
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

            if(startingPoints[0])
            {
                MLDrivers[i].transform.position = new Vector3(startingPoints[0].position.x, startingPoints[0].position.y, startingPoints[0].position.z);
                MLDrivers[i].transform.rotation = startingPoints[0].rotation;
            }
        }
    }

    public void DriverCrossedCheckpoint(MLDriver driver, int CheckpointID)
    {
        if(MLDrivers[driver.driverID].checkpointNum == CheckpointID - 1)
        {
            Debug.Log(driver.gameObject.name + " passed through " + CheckpointID.ToString());
            MLDrivers[driver.driverID].checkpointNum = CheckpointID;
            MLDrivers[driver.driverID].AddReward(checkpointReward);
        }
        else
        {
            //Punishing them for going backwards
            MLDrivers[driver.driverID].AddReward(-50f);
        }
    }

    public void DriverCrossedFinishLine(MLDriver driver)
    {
        if(MLDrivers[driver.driverID].checkpointNum == checkpoints.Count)
        {
            MLDrivers[driver.driverID].lapCount++;
            MLDrivers[driver.driverID].checkpointNum = 0;
            MLDrivers[driver.driverID].AddReward(lapReward);

            finishedDrivers++;

            //Temporary code for training
            UI.UpdateLapCounter(MLDrivers[driver.driverID].lapCount);
            UI.StopLapTimer();

            MLDrivers[driver.driverID].EndEpisode();
        }

        if(finishedDrivers == MLDrivers.Count)
        {
            StartCoroutine(Co_StartNextRace());
        }
    }

    public void DriverCrossedCheckpoint(PlayerDriver driver, int CheckpointID)
    {
        if(driver.checkpointNum == CheckpointID - 1)
        {
            driver.checkpointNum++;
        }
    }

    public void DriverCrossedFinishLine(PlayerDriver driver)
    {
        if(driver.checkpointNum == checkpoints.Count)
        {
            driver.lapCount++;
            driver.checkpointNum = 0;
        }
    }

    #endregion

    #region Private Methods

    private void StartRace()
    {
        //Line cars up and resets their variables
        ResetCars();

        UI.StartLapTimer();

        //Start Machine Learning Drivers
        foreach(MLDriver driver in MLDrivers)
        {
            driver.OnEpisodeBegin();
        }
    }

    private IEnumerator Co_StartNextRace()
    {
        yield return new WaitForSeconds(3.0f);
        StartRace();
    }

    #endregion
}
