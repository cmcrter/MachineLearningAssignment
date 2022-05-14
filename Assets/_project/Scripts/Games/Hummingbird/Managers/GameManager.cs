﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Manages game logic and controls the UI
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("Game ends when an agent collects this much nectar")]
    public float maxNectar = 8f;

    [Tooltip("Game ends after this many seconds have elapsed")]
    public float timerAmount = 90f;

    [Tooltip("The UI Controller")]
    public UIController uiController;

    [Tooltip("The player hummingbird")]
    public HummingbirdAgent player;

    [Tooltip("The ML-Agent opponent hummingbird")]
    public HummingbirdAgent opponent;

    [Tooltip("The flower area")]
    public FlowerArea flowerArea;

    [Tooltip("The main camera for the scene")]
    public Camera mainCamera;

    // When the game timer started
    private float gameTimerStartTime;

    /// <summary>
    /// Gets the time remaining in the game
    /// </summary>
    public float TimeRemaining
    {
        get
        {
            float timeRemaining = timerAmount - (Time.time - gameTimerStartTime);
            return Mathf.Max(0f, timeRemaining);
        }
    }

    /// <summary>
    /// Handles a button click in different states
    /// </summary>
    public void ButtonClicked()
    {
        StartGame();
    }

    /// <summary>
    /// Called when the game starts
    /// </summary>
    private void Start()
    {
        // Subscribe to button click events from the UI
        uiController.OnButtonClicked += ButtonClicked;
    }

    /// <summary>
    /// Called on destroy
    /// </summary>
    private void OnDestroy()
    {
        // Unsubscribe from button click events from the UI
        uiController.OnButtonClicked -= ButtonClicked;
    }

    /// <summary>
    /// Starts the game with a countdown
    /// </summary>
    /// <returns>IEnumerator</returns>
    private void StartGame()
    {
        // Update the UI (hide it)
        uiController.ShowBanner("");
        uiController.HideButton();

        // Use the player camera, disable the main camera
        mainCamera.gameObject.SetActive(false);
        player.agentCamera.gameObject.SetActive(true);

        // Start the game timer
        gameTimerStartTime = Time.time;

        // Reset the flowers
        flowerArea.ResetFlowers();

        // Reset the agents
        player.OnEpisodeBegin();
        opponent.OnEpisodeBegin();

        // Unfreeze the agents
        player.UnfreezeAgent();
        opponent.UnfreezeAgent();
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    private void EndGame()
    {
        StartGame();
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        // Check to see if time has run out or either agent got the max nectar amount
        if(TimeRemaining <= 0f ||
            player.NectarObtained >= maxNectar ||
            opponent.NectarObtained >= maxNectar)
        {
            EndGame();
        }

        // Update the timer and nectar progress bars
        uiController.SetTimer(TimeRemaining);
        uiController.SetPlayerNectar(player.NectarObtained / maxNectar);
        uiController.SetOpponentNectar(opponent.NectarObtained / maxNectar);
    }
}