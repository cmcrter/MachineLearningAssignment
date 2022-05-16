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

        [SerializeField]
        private bool isCirclesTurn = false;

        [SerializeField]
        private TTTBoard BoardValues;
        [SerializeField]
        private TTTBoardInteraction BoardUI;

        [SerializeField]
        private GAMEMODE mode = GAMEMODE.PvP;

        [SerializeField]
        private GameObject MLAgentX;
        private iAgent agentScriptX;

        [SerializeField]
        private GameObject MLAgentO;
        private iAgent agentScriptO;

        private int[] WinScores = new int[3];

        #endregion

        #region Unity Methods

        private void Awake()
        {
            BoardUI = BoardUI ?? FindObjectOfType<TTTBoardInteraction>();
            BoardValues = new TTTBoard();

            //Getting the agents
            if(MLAgentX)
            {
                if(MLAgentX.TryGetComponent<iAgent>(out var AgentComp))
                {
                    agentScriptX = AgentComp;
                    agentScriptX.SetTeam(Tile_State.X);
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

            //Flipping the turn bool
            isCirclesTurn = !isCirclesTurn;

            //Prompting the AI
            if(MLAgentO.activeSelf)
            {
                agentScriptO.SetTurn(isCirclesTurn);
            }

            if(MLAgentX.activeSelf)
            {
                agentScriptX.SetTurn(!isCirclesTurn);
            }

            //Enabling/Disabling the buttons if there's a player
            if(!isCirclesTurn && mode == GAMEMODE.PvAI)
            {
                BoardUI.EnableFreeBoardUI(BoardValues);
            }
            else if (isCirclesTurn && mode == GAMEMODE.PvAI)
            {
                BoardUI.DisableBoardUI();
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

        //Checking how many moves are left, if there's only 1, the AI doesn't need to calculate
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

        public float ReturnDrawAmount(Tile_State team)
        {
            int XAmount = 0;
            int OAmount = 0;

            //Counting the amount of claimed squares
            for(int i = 0; i < 3; ++i)
            {
                for(int j = 0; j < 3; ++j)
                {
                    if(BoardValues.boardIndexes[i, j] == (int)Tile_State.O)
                    {
                        OAmount++;
                    }
                    else if(BoardValues.boardIndexes[i, j] == (int)Tile_State.X)
                    {
                        XAmount++;
                    }
                }
            }

            //Giving the AI some reward if they drew based on the amount of claimed squares
            if(team == Tile_State.O)
            {
                if(OAmount > XAmount)
                {
                    return 0.75f;
                }
                else
                {
                    return -0.25f;
                }
            }

            if(team == Tile_State.X)
            {
                if(XAmount > OAmount)
                {
                    return 0.75f;
                }
                else
                {
                    return -0.25f;
                }
            }

            return 0;
        }

        #endregion

        #region Private Methods

        private void SetupGame()
        {
            BoardValues = new TTTBoard();

            switch(mode)
            {
                case GAMEMODE.PvAI:
                    //Player starts always
                    MLAgentO.SetActive(true);
                    agentScriptO.SetTurn(false);

                    BoardUI.EnableBoardUI();
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

            if(MLAgentO.activeSelf)
            {
                agentScriptO.Reinforce(endState);
            }

            if(MLAgentX.activeSelf)
            {
                agentScriptX.Reinforce(endState);
            }

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
            yield return new WaitForSeconds(0.1f);
            BoardUI.ClearUI();
            isCirclesTurn = false;
            SetupGame();
        }

        #endregion
    }

}