////////////////////////////////////////////////////////////
// File: TTTBoardInteraction.cs
// Author: Charles Carter
// Date Created: 03/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 10/02/22
// Brief: How scripts will interact with the board (as well as the player)
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ML.TTT
{
    public class TTTBoardInteraction : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private GameObject CirclePrefab;
        [SerializeField]
        private GameObject CrossPrefab;

        [SerializeField]
        private List<Transform> TileParents;
        [SerializeField]
        private List<Button> TileButtons;

        [SerializeField]
        private TextMeshProUGUI WinText;

        //0 - Draw
        //1 - X Wins
        //2 - O Wins
        [SerializeField]
        private List<TextMeshProUGUI> winTexts;

        #endregion

        #region Public Methods

        //Placing a tile
        public void Place(Tile_State newTileState, int index)
        {
            //Could be cleaned up by showing a sprite instead of this, but since it's a small game the optimization is not necessary

            if(newTileState.Equals(Tile_State.O) && CirclePrefab != null)
            {
                Instantiate(CirclePrefab, TileParents[index]);
            }
            else if(newTileState.Equals(Tile_State.X) && CrossPrefab != null)
            {
                Instantiate(CrossPrefab, TileParents[index]);
            }
        }

        public void GameEnded(Tile_State endState)
        {
            DisableBoardUI();

            if(WinText != null)
            {
                switch(endState)
                {
                    case Tile_State.X:
                        WinText.text = "X Wins!";
                        break;
                    case Tile_State.O:
                        WinText.text = "O Wins!";
                        break;
                    case Tile_State.BLANK:
                        WinText.text = "Draw!";
                        break;
                    default:
                        //Shouldn't reach here
                        break;
                }
            }
        }

        public void ClearUI()
        {
            for(int i = 0; i < TileParents.Count; ++i) 
            {
                if(TileParents[i] == null)
                {
                    return;
                }

                if(TileParents[i].childCount > 0)
                {
                    Transform t = TileParents[i].GetChild(0);

                    if(t != null)
                    {
                        Destroy(t.gameObject);
                    }
                }
            }

            WinText.text = "Running!";
        }

        public void DisableBoardUI() 
        {
            for(int i = 0; i < TileButtons.Count; ++i)
            {
                TileButtons[i].interactable = false;
            }
        }

        public void EnableBoardUI()
        {
            for(int i = 0; i < TileButtons.Count; ++i)
            {
                TileButtons[i].interactable = true;
            }
        }

        //Only making buttons interactable if the slot is not filled already
        public void EnableFreeBoardUI(TTTBoard thisBoard)
        {
            for(int i = 0; i < TileButtons.Count; ++i)
            {
                Vector2 boardPos = TTTBoard.ConvertIndexTo2DPoint(i);

                if(thisBoard.boardIndexes[(int)boardPos.x, (int)boardPos.y] == 0)
                {
                    TileButtons[i].interactable = true;
                }
            }
        }

        //Updating the win text when there's a win
        public void AddWin(Tile_State state, int winCount)
        {
            if(winTexts[(int)state] == null)
            {
                return;
            }

            winTexts[(int)state].text = winCount.ToString();
        }

        #endregion
    }
}
