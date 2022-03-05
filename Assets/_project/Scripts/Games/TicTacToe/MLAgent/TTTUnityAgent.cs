////////////////////////////////////////////////////////////
// File: TTTUnityAgent.cs
// Author: Charles Carter
// Date Created: 07/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 10/02/22
// Brief: A Unity ML Agent to play TicTacToe
//////////////////////////////////////////////////////////// 

using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace ML.TTT
{
    public class TTTUnityAgent : Agent, iAgent
    {
        void iAgent.Reinforce(Tile_State endState) => CollectReward(endState);
        void iAgent.SetTeam(Tile_State newTeam) => team = newTeam;
        void iAgent.SetTurn(bool isNowMyTurn) => SetTurn(isNowMyTurn);

        #region Variables

        private Tile_State team;

        [SerializeField]
        private TTTGameManager gameState;
        [SerializeField]
        private bool isMyTurn;

        #endregion

        #region Public Methods

        public override void OnEpisodeBegin()
        {
            
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
           //No heuristics needed
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            //Takes in an action by the player or AI
            if(isMyTurn)
            {
                int index = actions.DiscreteActions[0];
                gameState.MoveTaken(index);
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            //Getting the state of the board
            int[,] currentBoard = gameState.RetrieveBoard();

            sensor.AddObservation((int)team);

            sensor.AddObservation(currentBoard[0, 0]);
            sensor.AddObservation(currentBoard[1, 0]);
            sensor.AddObservation(currentBoard[2, 0]);

            sensor.AddObservation(currentBoard[0, 1]);
            sensor.AddObservation(currentBoard[1, 1]);
            sensor.AddObservation(currentBoard[2, 1]);

            sensor.AddObservation(currentBoard[0, 2]);
            sensor.AddObservation(currentBoard[1, 2]);
            sensor.AddObservation(currentBoard[2, 2]);
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            int[,] currentBoard = gameState.RetrieveBoard();

            for(int i = 0; i < currentBoard.Length; ++i)
            {
                Vector2 pos = TTTBoard.ConvertIndexTo2DPoint(i);
                bool isFilled = true ? currentBoard[(int)pos.x, (int)pos.y] != 0 : false;

                actionMask.SetActionEnabled(0, i, !isFilled);
            }
        }

        public void CollectReward(Tile_State teamWon)
        {
            if (team == teamWon)
            {
                AddReward(1.0f);
            }
            else if (teamWon == Tile_State.BLANK)
            {
                AddReward(gameState.ReturnDrawAmount(team));
            }
            else
            {
                AddReward(-1.0f);
            }

            EndEpisode();           
        } 

        #endregion

        #region Private Methods

        private void SetTurn(bool thisTurn)
        {
            isMyTurn = thisTurn;

            if(thisTurn)
            {
                StartCoroutine(Co_WaitTime());
            }
        }

        private IEnumerator Co_WaitTime()
        {
            yield return new WaitForSeconds(0.05f);
            RequestDecision();
        }

        #endregion
    }
}
