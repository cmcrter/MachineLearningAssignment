////////////////////////////////////////////////////////////
// File: MLInputManager.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: The Machine Learning Agent's action outcomes
//////////////////////////////////////////////////////////// 

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public enum ShooterGameOutcome
{
    Win,
    Lose,
    Draw
}

[RequireComponent(typeof(CharacterActions))]
public class MLShooter : Agent, IAgentable
{
    #region Interface Contracts

    void IAgentable.Reinforce(int state) => CollectReward(state);
    void IAgentable.SetTeam(int newTeam) => SetTeam();
    void IAgentable.SetTurn(bool isMyTurn) => SetTurn();

    #endregion

    #region Variables

    [SerializeField]
    private ShooterInstanceManager instanceManager;    

    [SerializeField]
    private CharacterActions actions;

    [SerializeField]
    private CharacterPickupTrigger characterPickup;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        actions = actions ?? GetComponent<CharacterActions>();
    }

    #endregion

    #region Override Methods

    public override void OnEpisodeBegin()
    {

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Action List - 1 discrete branch with 9 actions
        // 0 - do nothing (Action)
        // 1 - Turning (Action type)
        // 1.1 - Left (Action)
        // 1.2 - Right (Action)
        // 2 - Movement (Action type)
        // 2.1 - Forward (Action)
        // 2.2 - Backward (Action)
        // 2.3 - Left (Action)
        // 3.4 - Right (Action)
        // 3 - Equip (Action type)
        // 3.1 - Shoot (Action)
        // 3.2 - Drop (Action)
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Environment Objects within your FOV (the position amounts should be the same)
        //for()
        //{
        //  Looping through the objects seen and adding the relative positions and what they are 
        //}

        //Important Objects visible out of potential objects visible (including type), the number should be the same each time
        //for()
        //{
        //  Looping through the guns available
        //}

        //Adding if the AI is holding a gun
        //Relative position of the barrel to the AI

        //Direction faced by this AI

        //Position of this AI

        //Health

        //Environment Observations (Could be incomplete)
        //for()
        //{
        //  Looping through the bullets, and seeing where they are and which direction they're going and what speed (reserve space for all potential used size of object pools)
        //}
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        if(characterPickup)
            return;

        bool hasWeapon = characterPickup.bEquipped;

        //The only actions blocked should be when the agent has no gun
        actionMask.SetActionEnabled(0, 7, hasWeapon);
        actionMask.SetActionEnabled(0, 8, hasWeapon);
    }

    #endregion

    #region Public Methods

    //Will be the same int value as the layer
    public void SetTeam()
    {
    }

    //Not necessary for this agent
    public void SetTurn()
    {
    }

    public void AddScore(float amount)
    {
        //Easier to add a negative score for missing shots and the like
    }

    public void CollectReward(int endState)
    {
        //Round Draw
        if(endState == (int)ShooterGameOutcome.Draw)
        {
            
        }
        //Round Win
        else if (endState == (int)ShooterGameOutcome.Win)
        {
        
        }
        //Round Lose
        else if(endState == (int)ShooterGameOutcome.Lose)
        {

        }

        EndEpisode();
    }

    #endregion
}
