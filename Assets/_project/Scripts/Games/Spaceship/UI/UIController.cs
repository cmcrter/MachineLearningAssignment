using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a very simple UI. Doesn't do anything on its own.
/// </summary>
public class UIController : MonoBehaviour
{
    [Tooltip("The Information progress bar for the player")]
    public Slider playerInformationBar;

    [Tooltip("The Information progress bar for the opponent")]
    public Slider opponentInformationBar;

    [Tooltip("The timer text")]
    public TextMeshProUGUI timerText;

    /// <summary>
    /// Delegate for a button click
    /// </summary>
    public delegate void ButtonClick();

    /// <summary>
    /// Called when the button is clicked
    /// </summary>
    public ButtonClick OnButtonClicked;

    /// <summary>
    /// Responds to button clicks
    /// </summary>
    public void ButtonClicked()
    {
        if (OnButtonClicked != null) OnButtonClicked();
    }

    /// <summary>
    /// Sets the timer, if timeRemaining is negative, hides the text
    /// </summary>
    /// <param name="timeRemaining">The time remaining in seconds</param>
    public void SetTimer(float timeRemaining)
    {
        //Returns the timer in minutes/seconds
        (float, float) timerVal = Timer.TimerFormatter(timeRemaining);

        timerText.text = timerVal.Item1.ToString("00") + ":" + timerVal.Item2.ToString("00");
    }

    /// <summary>
    /// Sets the player's Information amount
    /// </summary>
    /// <param name="InformationAmount">An amount between 0 and 1</param>
    public void SetPlayerInformation(float InformationAmount)
    {
        if(!playerInformationBar)
        {
            return;
        }

        playerInformationBar.value = InformationAmount;
    }

    /// <summary>
    /// Sets the opponent's Information amount
    /// </summary>
    /// <param name="InformationAmount">An amount between 0 and 1</param>
    public void SetOpponentInformation(float InformationAmount)
    {
        opponentInformationBar.value = InformationAmount;
    }
}
