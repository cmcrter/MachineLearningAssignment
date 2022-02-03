////////////////////////////////////////////////////////////
// File: TTTBoard.cs
// Author: Charles Carter
// Date Created: 03/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 03/02/2022
// Brief: The general board that the game uses
//////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ML.TTT
{
    public enum Tile_State
    {
        BLANK = 0,
        X = 1,
        O = 2
    }

    public enum BoardState
    {
        RUNNNING = 0,
        XWIN = 1,
        OWIN = 2,
        DRAW = 3,
    }

    [Serializable]
    public class TTTBoard
    {
        #region Variables

        public static int BOARD_WIDTH = 3;
        public static int BOARD_COUNT = 9;

        public int[] boardIndexes;
        public BoardState boardState;

        #endregion

        #region Constructor/Destructor

        public TTTBoard()
        {
            //Sets a blank board
            boardIndexes = new int[9];
            boardState = BoardState.RUNNNING;
        }

        #endregion

        #region Public Methods

        public void TilePlaced(Tile_State state, int index)
        {
            boardIndexes[index] = (int)state;
            CheckGameState();
        }

        public static int[] GetNeighbours(int tileX, int tileY)
        {
            List<int> neighbours = new List<int>();

            for(int x = -1; x < 1; x++) 
            {
                for(int y = -1; y < 1; y++)
                {
                    //Ignore the tile inputted
                    if(x == 0 && y == 0 || tileX > BOARD_WIDTH || tileX < 0 || tileY > BOARD_WIDTH || tileY < 0)
                        continue;

                    int thisIndex = Convert2DPointToIndex(tileX + x, tileY + y);
                    neighbours.Add(thisIndex);
                }
            }

            return neighbours.ToArray();
        }

        public static int Convert2DPointToIndex(int tileX, int tileY)
        {
            return (tileX * BOARD_WIDTH) + tileY;
        }

        public static Vector2 ConvertIndexTo2DPoint(int index)
        {
            return new Vector2(index % BOARD_WIDTH, index / BOARD_WIDTH);
        }

        #endregion

        #region Private Methods

        private void CheckGameState()
        {
            bool bEmptyTile = false;

            // Check columns
            for(int x = 0; x < BOARD_WIDTH; x++)
            {
                Tile_State intitalState = (Tile_State)boardIndexes[x];

                if(CheckColumn(intitalState, x))
                {
                    boardState = (BoardState)intitalState;
                    return;
                }
            }

            // Check rows
            for(int y = 0; y < BOARD_COUNT; y++)
            {
                Tile_State intitalState = (Tile_State)boardIndexes[y];

                if(CheckRow(intitalState, y))
                {
                    boardState = (BoardState)intitalState;
                    return;
                }
            }

            // Check diagonals
            if(CheckDiagonals())
            {
                boardState = (BoardState)boardIndexes[4];
                return;
            }

            //Checking whether it's a draw
            for(int i = 0; i < BOARD_COUNT; i++)
            {
                if(boardIndexes[i] == 0)
                {
                    bEmptyTile = true;
                }
            }

            if(!bEmptyTile)
            {
                boardState = BoardState.DRAW;
                return;
            }
            
            boardState = BoardState.RUNNNING;
        }

        public bool CheckColumn(Tile_State intitalState, int startX)
        {
            for(int i = 0; i < BOARD_WIDTH; i++)
            {
                int index = Convert2DPointToIndex(startX, i);

                if(index < 0 || index > BOARD_COUNT)
                {
                    break;
                }

                if((Tile_State)boardIndexes[index] != intitalState)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckRow(Tile_State intitalState, int startY)
        {
            for(int i = 0; i < BOARD_WIDTH; i++)
            {
                int index = Convert2DPointToIndex(i, startY);

                if(index < 0 || index > BOARD_COUNT)
                {
                    break;
                }

                if((Tile_State)boardIndexes[index] != intitalState)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckDiagonals()
        {
            Tile_State middleState = (Tile_State)boardIndexes[4];

            if((Tile_State)boardIndexes[0] == middleState && (Tile_State)boardIndexes[8] == middleState)
            {
                return true;
            }
            else if((Tile_State)boardIndexes[2] == middleState && (Tile_State)boardIndexes[6] == middleState)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}