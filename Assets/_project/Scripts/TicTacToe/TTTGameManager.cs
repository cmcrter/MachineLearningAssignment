////////////////////////////////////////////////////////////
// File: TTTGameManager
// Author: Charles Carter
// Date Created: 03/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 03/02/2022
// Brief: The overall game manager which runs Tic Tac Toe
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace ML.TTT
{
    public enum GAMEMODE
    {
        PvP = 0,
        PvAI = 1,
        AIvAI = 2
    }

    public class TTTGameManager : MonoBehaviour
    {
        #region Variables

        private bool isCirclesTurn = false;

        [SerializeField]
        private TTTBoard BoardValues;
        [SerializeField]
        private TTTBoardInteraction BoardUI;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            BoardUI = BoardUI ?? FindObjectOfType<TTTBoardInteraction>();
            BoardValues = new TTTBoard();
        }

        #endregion

        #region Public Methods

        public void MoveTaken(int Index)
        {
            Tile_State state = isCirclesTurn ? Tile_State.O : Tile_State.X;
    
            //Updating the values
            BoardValues.TilePlaced(state, Index);

            //Updating the UI
            BoardUI.Place(state, Index);

            isCirclesTurn = !isCirclesTurn;

            //Checking if the board state means the game is over
            if(BoardValues.boardState != BoardState.RUNNNING)
            {
                BoardUI.GameEnded(BoardValues.boardState);
            }
        }

        #endregion

        #region Private Methods

        private void SetupGame()
        {
            BoardValues = new TTTBoard();
        }

        #endregion
    }

}