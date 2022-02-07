////////////////////////////////////////////////////////////
// File: TTTUnityAgent.cs
// Author: Charles Carter
// Date Created: 07/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 07/02/22
// Brief: A Unity ML Agent to play TicTacToe
//////////////////////////////////////////////////////////// 

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
        private bool isMyTurn;

        #endregion

        #region Public Methods

        public override void OnEpisodeBegin()
        {
            gameState.ResetGame();
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
           //No heuristics needed
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            //Takes in an action by the player or AI
            Debug.Log(actions.DiscreteActions[0]);

            if(isMyTurn)
            {
                int index = actions.DiscreteActions[0];

                if(gameState.canPlaceThere(index))
                {
                    gameState.MoveTaken(index);
                }
                else
                {
                    //Get random free square and use it
                    gameState.MoveTaken(-1);
                }
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

        public void CollectReward(Tile_State teamWon)
        {
            if (team == teamWon)
            {
                AddReward(1.0f);
            }
            else if (teamWon == Tile_State.BLANK)
            {
                AddReward(0.1f);
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
                RequestDecision();
            }
        }

        #endregion
    }
}
