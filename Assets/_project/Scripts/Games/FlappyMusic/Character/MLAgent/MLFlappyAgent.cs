////////////////////////////////////////////////////////////
// File: MLFlappyAgent.cs
// Author: Charles Carter
// Date Created: 08/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 08/03/22
// Brief: The Machine Learning agent component
//////////////////////////////////////////////////////////// 

using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
public class MLFlappyAgent : Agent, IAgentable
{
    #region Interface Contracts

    void IAgentable.Reinforce(int reward) => CollectReward(reward);
    void IAgentable.SetTeam(int newTeam) => PerformNothing();
    void IAgentable.SetTurn(bool isMyTurn) => PerformNothing();

    #endregion

    #region Variables

    [SerializeField]
    private WorldMover manager;

    [SerializeField]
    private BirdMovement CharacterActions;

    private bool tappedLastFrame = false;
    private float height = 8f;

    #endregion

    #region Unity Methods

    void FixedUpdate()
    {
        //The longer the agent survives, the more reward
        AddReward(Time.fixedDeltaTime);
    }

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
        //1 action, to tap or not
        tappedLastFrame = Mathf.FloorToInt(actions.DiscreteActions[0]) == 1 ? true : false;

        if(tappedLastFrame)
        {
            CharacterActions.Tap();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //The height is the distance from the bottom to the center (used to normalize the values)
        Vector3 nextPipePos = manager.GetNextPipe().localPosition;
        float vel = Mathf.Clamp(CharacterActions.GetYVel(), -height, height);

        sensor.AddObservation(transform.localPosition.y / height);
        sensor.AddObservation(vel / height);
        sensor.AddObservation(nextPipePos.y / height);
        sensor.AddObservation(nextPipePos.x);
        sensor.AddObservation(tappedLastFrame ? 1f : -1f);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {

    }


    #endregion

    #region Private Methods

    private void CollectReward(int reward)
    {
        AddReward(reward);
    }

    private void PerformNothing()
    {

    }

    #endregion
}
