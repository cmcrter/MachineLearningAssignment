////////////////////////////////////////////////////////////
// File: TTTGameManager
// Author: Charles Carter
// Date Created: 03/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 03/02/2022
// Brief: The overall game manager which runs Tic Tac Toe
//////////////////////////////////////////////////////////// 

using System;
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

        [SerializeField]
        GAMEMODE mode = GAMEMODE.PvP;

        [SerializeField]
        GameObject MLAgentX;
        iAgent agentScriptX;

        [SerializeField]
        GameObject MLAgentO;
        iAgent agentScriptO;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            BoardUI = BoardUI ?? FindObjectOfType<TTTBoardInteraction>();
            BoardValues = new TTTBoard();

            if(MLAgentX)
            {
                if(MLAgentX.TryGetComponent<iAgent>(out var AgentComp))
                {
                    agentScriptX = AgentComp;
                    agentScriptX.SetTeam(Tile_State.O);
                }
            }

            if(MLAgentO)
            {
                if(MLAgentO.TryGetComponent<iAgent>(out var AgentComp))
                {
                    agentScriptO = AgentComp;
                    agentScriptO.SetTeam(Tile_State.O);
                }
            }
        }

        private void OnEnable()
        {
            SetupGame();

            switch(mode)
            {
                case GAMEMODE.PvAI:
                    MLAgentO.SetActive(true);
                    break;
                case GAMEMODE.AIvAI:
                    MLAgentO.SetActive(true);
                    MLAgentX.SetActive(true);
                    break;
            }
        }

        #endregion

        #region Public Methods

        public void MoveTaken(int index)
        {
            Tile_State state = isCirclesTurn ? Tile_State.O : Tile_State.X;
            Vector2 pos = TTTBoard.ConvertIndexTo2DPoint(index);
            Debug.Log(pos + " " + state.ToString());

            //Updating the values
            BoardValues.TilePlaced(state, (int)pos.x, (int)pos.y);

            //Updating the UI
            BoardUI.Place(state, index);

            //Checking if the board state means the game is over
            (bool, Tile_State) currentState = BoardValues.isGameOver();

            if(currentState.Item1)
            {
                state = currentState.Item2;

                BoardUI.GameEnded(state);
                EndGameForAI();
                return;
            }

            isCirclesTurn = !isCirclesTurn;

            if(mode == GAMEMODE.PvAI || mode == GAMEMODE.AIvAI)
            {
                if(isCirclesTurn)
                {
                    agentScriptO.SetTurn(true);
                    agentScriptX.SetTurn(false);
                }
                else
                {
                    agentScriptX.SetTurn(true);
                    agentScriptO.SetTurn(false);
                }
            }
        }

        public void ResetGame()
        {

        }

        public int[,] RetrieveBoard()
        {
            return BoardValues.boardIndexes;
        }

        public bool canPlaceThere(int indexer)
        {
            Vector2 pos = TTTBoard.ConvertIndexTo2DPoint(indexer);

            if(BoardValues.boardIndexes[(int)pos.x, (int)pos.y] != 0)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void SetupGame()
        {
            BoardValues = new TTTBoard();
        }

        private void EndGameForAI()
        {
            if(mode == GAMEMODE.PvP)
            {
                return;
            }


        }

        #endregion
    }

}