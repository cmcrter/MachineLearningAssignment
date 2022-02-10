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

        public void AddWin(Tile_State state, int winCount)
        {
            if(winTexts[(int)state] == null)
            {
                return;
            }

            winTexts[(int)state].text = winCount.ToString();
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
