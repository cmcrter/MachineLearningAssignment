////////////////////////////////////////////////////////////
// File: FlappyUI.cs
// Author: Charles Carter
// Date Created: 14/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 14/05/22
// Brief: A script to control the UI in the FlappyMusic Demo scene
//////////////////////////////////////////////////////////// 

using TMPro;
using UnityEngine;

public enum FlappyGameResult
{
    PlayerWon,
    PlayerLost
}

public class FlappyUI : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private WorldMover mover;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI resultText;

    #endregion

    #region Public Methods

    public void GameOverUISetValues(int score, FlappyGameResult result)
    {
        if(resultText)
        {
            if(result == FlappyGameResult.PlayerLost)
            {
                resultText.text = "Player Lost!";
            }
            else
            {
                resultText.text = "Player Won!";
            }
        }

        if(scoreText)
        {
            scoreText.text = score.ToString();
        }
    }

    #endregion
}
