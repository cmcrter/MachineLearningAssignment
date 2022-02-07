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

    [Serializable]
    public class TTTBoard
    {
        #region Variables

        public static int BOARD_WIDTH = 3;
        public static int BOARD_COUNT = 9;

        public int[,] boardIndexes;

        #endregion

        #region Constructor/Destructor

        public TTTBoard()
        {
            //Sets a blank board
            boardIndexes = new int[3,3];
        }

        #endregion

        #region Public Methods

        public void TilePlaced(Tile_State state, int tileX, int tileY)
        {
            boardIndexes[tileX, tileY] = (int)state;
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
            return tileX + BOARD_WIDTH * tileY;
        }

        public static Vector2 ConvertIndexTo2DPoint(int index)
        {
            return new Vector2(index % BOARD_WIDTH, index / BOARD_WIDTH);
        }

        #endregion

        #region Private Methods

        public (bool, Tile_State) isGameOver()
        {
            bool bEmptyTile = false;

            // Check columns
            for(int x = 0; x < BOARD_WIDTH; x++)
            {
                Tile_State intitalState = (Tile_State)boardIndexes[x, 0];

                if(CheckColumn(intitalState, x))
                {
                    return (true, intitalState);
                }
            }

            // Check rows
            for(int y = 0; y < BOARD_WIDTH; y++)
            {
                Tile_State intitalState = (Tile_State)boardIndexes[0, y];

                if(CheckRow(intitalState, y))
                {
                    return (true, intitalState);
                }
            }

            // Check diagonals
            if((Tile_State)boardIndexes[1, 1] != Tile_State.BLANK && CheckDiagonals())
            {
                return (true, (Tile_State)boardIndexes[1, 1]);
            }

            //Checking whether it's a draw
            for(int i = 0; i < BOARD_WIDTH; i++)
            {
                for(int j = 0; j < BOARD_WIDTH; j++)
                {
                    if(boardIndexes[i, j] == 0)
                    {
                        bEmptyTile = true;
                    }
                }
            }

            if(!bEmptyTile)
            {
                return (true, Tile_State.BLANK);
            }

            return (false, Tile_State.BLANK);
        }

        public bool CheckColumn(Tile_State intitalState, int startX)
        {
            if(intitalState == Tile_State.BLANK)
            {
                return false;
            }

            if((Tile_State)boardIndexes[startX, 1] != intitalState || (Tile_State)boardIndexes[startX, 2] != intitalState)
            {
                return false;
            }

            return true;
        }

        public bool CheckRow(Tile_State intitalState, int startY)
        {
            if(intitalState == Tile_State.BLANK)
            {
                return false;
            }

            if((Tile_State)boardIndexes[1, startY] != intitalState || (Tile_State)boardIndexes[2, startY] != intitalState)
            {
                return false;
            }

            return true;
        }

        public bool CheckDiagonals()
        {
            Tile_State middleState = (Tile_State)boardIndexes[1, 1];

            if((Tile_State)boardIndexes[0, 0] == middleState && (Tile_State)boardIndexes[2, 2] == middleState)
            {
                return true;
            }
            else if((Tile_State)boardIndexes[0, 2] == middleState && (Tile_State)boardIndexes[2, 0] == middleState)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}