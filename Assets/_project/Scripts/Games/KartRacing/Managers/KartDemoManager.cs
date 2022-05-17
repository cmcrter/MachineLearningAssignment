////////////////////////////////////////////////////////////
// File: KartDemoManager.cs
// Author: Charles Carter
// Date Created: 14/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 14/05/22
// Brief: The UI for the demo scene for Kart Racing
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KartDemoManager : MonoBehaviour
{
    #region Variables

    [Header("Showing Stage of Game")]
    public Timer bettingTimer;
    public Timer raceTimer;
    public bool RaceEnded = false;

    [SerializeField]
    private bool bOverrideBetting = false;
    
    [SerializeField]
    private KartDemoUI UI;

    public List<MLDriver> drivers = new List<MLDriver>();

    [SerializeField]
    private Transform startPos;

    //To be reused
    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Adjustments of Section Lengths")]
    [SerializeField]
    private float bettingDuration = 30f;
    [SerializeField]
    private float raceDuration = 900f;

    public List<int> carsWhoCrossedTheLine = new List<int>();

    [SerializeField]
    private float timeScale = 1f;

    [SerializeField]
    private List<Transform> checkpoints = new List<Transform>();

    #endregion

    #region Unity Methods

    void Start()
    {
        StartCoroutine(Co_GameLoop());
    }

    #endregion

    #region Public Methods

    public void DriverCrossedCheckpoint(MLDriver driver, int checkpointID)
    {
        if(driver.checkpointNum == checkpointID)
        {
            driver.checkpointNum = checkpointID + 1;
        }
    }

    public void DriverCrossedFinish(MLDriver driver)
    {
        if(driver.checkpointNum == checkpoints.Count)
        {
            driver.checkpointNum = 0;
            carsWhoCrossedTheLine.Add(driver.driverID);
            driver.enabled = false;
        }

        if(carsWhoCrossedTheLine.Count == drivers.Count)
        {
            RaceEnded = true;
        }
    }

    public void LockGame(bool locking)
    {
        bettingTimer.isLocked = locking;
        raceTimer.isLocked = locking;
    }

    public void StartRaceEarly()
    {
        if(bettingTimer != null)
        {
            bettingTimer.OverrideCurrentTime(-bettingTimer.current_time);
            bOverrideBetting = true;
        }
    }

    public void StartNextRound()
    {
        StartCoroutine(Co_GameLoop());
    }

    #endregion

    #region Private Methods

    //Seperating the game into timed stages
    private IEnumerator Co_GameLoop()
    {
        SetupBetting();

        yield return StartCoroutine(Co_BettingTimer());

        SetupRace();

        yield return StartCoroutine(Co_RoundTimer());

        RaceEndScreen();
    }

    private IEnumerator Co_BettingTimer()
    {
        bettingTimer = new Timer(bettingDuration);
        UpdateTimerUI(timerText, bettingTimer);

        while(bettingTimer.isActive && !bOverrideBetting)
        {
            bettingTimer.Tick(Time.deltaTime);
            UpdateTimerUI(timerText, bettingTimer);
            yield return null;
        }

        bOverrideBetting = false;
    }

    private IEnumerator Co_RoundTimer()
    {
        raceTimer = new Timer(raceDuration);
        UpdateTimerUI(timerText, raceTimer);

        RaceEnded = false;

        Time.timeScale = timeScale;

        while(raceTimer.isActive && !RaceEnded)
        {
            raceTimer.Tick(Time.deltaTime);
            UpdateTimerUI(timerText, raceTimer);
            yield return null;
        }

        Time.timeScale = 1;
    }

    private void UpdateTimerUI(TextMeshProUGUI text, Timer timer)
    {
        if(!text)
        {
            return;
        }

        //Returns the timer in minutes/seconds
        (float, float) timerVal = Timer.TimerFormatter(timer.current_time);

        text.text = timerVal.Item1.ToString("00") + ":" + timerVal.Item2.ToString("00");
    }

    private void SetupRace()
    {
        //Put vehicles in position
        foreach(MLDriver driver in drivers)
        {
            driver.transform.position = startPos.transform.position;
            driver.transform.rotation = startPos.transform.rotation;
        }

        //Close Betting UI
        UI.CloseBettingPanel();

        //Enable them
        foreach(MLDriver driver in drivers)
        {
            driver.enabled = true;
        }
    }

    private void SetupBetting()
    {
        //Clear previous values
        UI.ResetBets();

        //Close Last Race UI if open
        UI.CloseRaceEndPanel();

        //Activate UI
        UI.OpenBettingPanel();
    }

    private void RaceEndScreen()
    {
        //Shut down all cars
        foreach(MLDriver driver in drivers)
        {
            driver.enabled = false;
        }

        //Once either timer has run out or all vehicles have crossed the line
        UI.CarWon(carsWhoCrossedTheLine);

        //Show UI
        UI.OpenRaceEndPanel();
    }

    #endregion
}
