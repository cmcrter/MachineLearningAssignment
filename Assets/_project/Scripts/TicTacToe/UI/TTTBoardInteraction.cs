////////////////////////////////////////////////////////////
// File: TTTBoardInteraction.cs
// Author: Charles Carter
// Date Created: 03/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 03/02/22
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

        #endregion

        #region Unity Methods

        private void Awake()
        {

        }

        #endregion

        #region Public Methods

        public void Place(Tile_State newTileState, int index)
        {
            if(newTileState.Equals(Tile_State.O) && CirclePrefab != null)
            {
                Instantiate(CirclePrefab, TileParents[index]);
            }
            else if(newTileState.Equals(Tile_State.X) && CrossPrefab != null)
            {
                Instantiate(CrossPrefab, TileParents[index]);
            }
        }

        public void GameEnded(BoardState endState)
        {
            DisableBoardUI();

            if(WinText != null)
            {
                switch(endState)
                {
                    case BoardState.XWIN:
                        WinText.text = "X Wins!";
                        break;
                    case BoardState.OWIN:
                        WinText.text = "O Wins!";
                        break;
                    case BoardState.DRAW:
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
                Destroy(TileParents[i].GetChild(0));
            }
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

        #endregion

        #region Private Methods
        #endregion
    }
}
