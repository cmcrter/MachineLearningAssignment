using System.Collections;
using UnityEngine;

/// <summary>
/// Manages game logic and controls the UI
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("General Game Information")]
    [Tooltip("Game ends when an agent collects this much Information")]
    public float maxInformation = 8f;

    [Tooltip("Game ends after this many seconds have elapsed")]
    public float timerAmount = 90f;

    [Tooltip("The UI Controller")]
    public UIController uiController;

    [Tooltip("The player hummingbird")]
    public SpaceshipAgent player;

    [Tooltip("The ML-Agent opponent hummingbird")]
    public SpaceshipAgent opponent;

    [Tooltip("The computers area")]
    public ComputerArea computersArea;

    [Tooltip("The main camera for the scene")]
    public Camera mainCamera;

    private Timer gameTimer;
    private float gameDuration = 90f;

    [Header("Sounds Needed")]
    [SerializeField]
    SoundDetails informationGathered;

    [SerializeField]
    SoundDetails timerOver;

    /// <summary>
    /// Gets the time remaining in the game
    /// </summary>
    public float TimeRemaining
    {
        get
        {
            return gameTimer.current_time;
        }
    }

    /// <summary>
    /// Handles a button click in different states
    /// </summary>
    public void ButtonClicked()
    {
        StartGame();
    }

    private void Awake()
    {
        gameTimer = new Timer(gameDuration);
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
        // Start the game timer
        gameTimer = new Timer(gameDuration);

        // Reset the Computers
        computersArea.ResetComputers();

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
        // Check to see if time has run out or either agent got the max Information amount
        if(TimeRemaining <= 0f ||
            player.InformationObtained >= maxInformation ||
            opponent.InformationObtained >= maxInformation)
        {
            EndGame();
        }

        gameTimer.Tick(Time.deltaTime);

        // Update the timer and Information progress bars
        uiController.SetTimer(TimeRemaining);
        uiController.SetPlayerInformation(player.InformationObtained / maxInformation);
        uiController.SetOpponentInformation(opponent.InformationObtained / maxInformation);
    }
}
