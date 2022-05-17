////////////////////////////////////////////////////////////
// File: Timer.cs
// Author: Charles Carter
// Date Created: 30/09/21
// Last Edited By: Charles Carter
// Date Last Edited: 17/05/22
// Brief: A POCO script to handle timer floats
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;

//A class not attached to unity to handle the timer float
[Serializable]
public class Timer
{
    #region Variables Needed

    //Could be locked
    public bool isLocked;
    public bool isActive;

    //Needed to tick up or down
    public float max_time
    {
        get; private set;
    }

    public float current_time
    {
        get; private set;
    }

    #endregion

    #region Public Methods

    //Constructor
    public Timer(float duration)
    {
        max_time = duration;
        current_time = duration;

        isActive = true;
    }

    ~Timer()
    {
        //Garbage collection should do this (could implement a dispose if needed)
    }

    //Ticking the timer down
    public void Tick(float delta_time)
    {
        if(current_time == 0 || isLocked)
        {
            return;
        }

        current_time -= delta_time;
        EndCheck();
    }

    //If something wants to add/remove time
    public void OverrideCurrentTime(float amountToChangeBy)
    {
        current_time += amountToChangeBy;
    }

    #endregion

    #region Private Methods

    //Function to check if timer ran out
    private void EndCheck()
    {
        //Timer is over
        if(current_time <= 0)
        {
            current_time = 0;
            isActive = false;
        }
    }

    //Function to return a time given in minutes and seconds
    public static (float, float) TimerFormatter(float time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time % 60);

        return (minutes, seconds);
    }

    #endregion
}