////////////////////////////////////////////////////////////
// File: RaceUI.cs
// Author: Charles Carter
// Date Created: 06/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/22
// Brief: A script to update the instances world canvas UI
//////////////////////////////////////////////////////////// 

using TMPro;
using UnityEngine;

public class RaceUI : MonoBehaviour
{
    #region Variables

    private Timer LapTimer;
    [SerializeField]
    private TextMeshProUGUI TimerText;

    [SerializeField]
    private TextMeshProUGUI LapText;

    #endregion

    #region Unity Methods
    void Start()
    {
        LapTimer = new Timer(0.01f);
    }
 
    void Update()
    {
        LapTimer.Tick(-Time.deltaTime);
        UpdateTimerUI();
    }
    #endregion

    #region Public Methods

    public void StartLapTimer()
    {
        LapTimer = new Timer(0.01f);
    }

    public void StopLapTimer()
    {
        LapTimer.isLocked = true;
    }

    public void UpdateLapCounter(int newLapAmount)
    {
        LapText.text = newLapAmount.ToString();
    }

    public float ReturnCurrentTime()
    {
        return LapTimer.current_time;
    }

    #endregion

    #region Private Methods

    private void UpdateTimerUI()
    {
        if(!LapText)
            return;

        float minutes = Mathf.Floor(LapTimer.current_time / 60);
        float seconds = Mathf.Floor(LapTimer.current_time % 60);

        TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    #endregion
}
