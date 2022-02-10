////////////////////////////////////////////////////////////
// File: TTTGameManager
// Author: Charles Carter
// Date Created: 03/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 10/02/2022
// Brief: The overall game manager which runs Tic Tac Toe
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using System.Collections.Generic;
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

        private int[] WinScores = new int[3];

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
        }

        #endregion

        #region Public Methods

        public void MoveTaken(int index)
        {
            if(index == -1)
            {
                index = GetRandomPos();
            }

            Tile_State state = isCirclesTurn ? Tile_State.O : Tile_State.X;
            Vector2 pos = TTTBoard.ConvertIndexTo2DPoint(index);
            //Debug.Log(pos + " " + state.ToString());

            //Updating the values
            BoardValues.TilePlaced(state, (int)pos.x, (int)pos.y);

            //Updating the UI
            BoardUI.Place(state, index);

            //Checking if the board state means the game is over
            (bool, Tile_State) currentState = BoardValues.isGameOver();

            if(currentState.Item1)
            {
                state = currentState.Item2;
                WinScores[(int)state]++;
    
                BoardUI.GameEnded(state);
                BoardUI.AddWin(state, WinScores[(int)state]);

                EndGameForAI(state);
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
            StartCoroutine(Co_WaitTime());
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

        public (bool, int) OneMoveLeft()
        {
            int spaceLeft = 0;
            int indexSpace = 0;

            for(int i = 0; i < 3; ++i)
            {
                for(int j = 0; j < 3; ++j)
                {
                    if(BoardValues.boardIndexes[i, j] == 0)
                    {
                        spaceLeft++;
                        indexSpace = TTTBoard.Convert2DPointToIndex(i, j);
                    }

                    if(spaceLeft > 1)
                    {
                        return (false, 0);
                    }
                }
            }

            return (true, indexSpace);
        }

        #endregion

        #region Private Methods

        private void SetupGame()
        {
            BoardValues = new TTTBoard();

            switch(mode)
            {
                case GAMEMODE.PvAI:
                    MLAgentO.SetActive(true);
                    break;
                case GAMEMODE.AIvAI:
                    MLAgentO.SetActive(true);
                    MLAgentX.SetActive(true);

                    agentScriptX.SetTurn(true);
                    agentScriptO.SetTurn(false);

                    BoardUI.DisableBoardUI();
                    break;
            }
        }

        private void EndGameForAI(Tile_State endState)
        {
            if(mode == GAMEMODE.PvP)
            {
                return;
            }

            agentScriptO.Reinforce(endState);
            agentScriptX.Reinforce(endState);

            MLAgentO.SetActive(false);
            MLAgentX.SetActive(false);

            ResetGame();
        }

        private int GetRandomPos()
        {
            List<(int, int)> freeSpaces = new List<(int, int)>();

            for(int i = 0; i < 3; ++i)
            {
                for(int j = 0; j < 3; ++j)
                {
                    if(BoardValues.boardIndexes[i, j] == 0)
                    {
                        freeSpaces.Add((i, j));
                    }
                }
            }

            (int, int) indexPoint = freeSpaces[UnityEngine.Random.Range(0, freeSpaces.Count)];

            return TTTBoard.Convert2DPointToIndex(indexPoint.Item1, indexPoint.Item2);
        }

        private IEnumerator Co_WaitTime()
        {
            yield return new WaitForSeconds(1.0f);
            BoardUI.ClearUI();
            isCirclesTurn = false;
            SetupGame();
        }

        #endregion
    }

}