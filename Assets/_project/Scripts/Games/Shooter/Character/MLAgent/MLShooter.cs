////////////////////////////////////////////////////////////
// File: MLInputManager.cs
// Author: Charles Carter
// Date Created: 22/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: The Machine Learning Agent's action outcomes
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
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

    public ShooterInstanceManager instanceManager;    

    [SerializeField]
    private CharacterActions characterActions;

    [SerializeField]
    private CharacterPickupTrigger characterPickup;

    [SerializeField]
    private CharacterSenses senses;

    [SerializeField]
    private CharacterHealth health;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        characterActions = characterActions ?? GetComponent<CharacterActions>();
        senses = senses ?? GetComponent<CharacterSenses>();
        health = health ?? GetComponent<CharacterHealth>();
    }

    #endregion

    #region Override Methods

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Action List - 1 discrete branch with 9 actions
        // 0 - do nothing (Action)
        switch(actions.DiscreteActions[0])
        {
            case 0:
                AddReward(-0.01f);
                return;
            case 1:
                // 1 - Turning (Action type)
                // 1.1 - Left (Action)
                characterActions.TurnLeft();
                break;
            case 2:
                // 1.2 - Right (Action)
                characterActions.TurnRight();
                break;
            case 3:
                // 2 - Movement (Action type)
                // 2.1 - Forward (Action)
                characterActions.MoveForward();
                break;
            case 4:
                // 2.2 - Backward (Action)
                characterActions.MoveBackward();
                break;
            case 5:
                // 2.3 - Left (Action)
                characterActions.MoveLeft();
                break;
            case 6:
                // 3.4 - Right (Action)
                characterActions.MoveRight();
                break;
            case 7:
                // 3 - Equip (Action type)
                // 3.1 - Shoot (Action)
                characterActions.Shoot();
                break;
            //case 8:
            //    // 3.2 - Drop (Action)
            //    characterActions.Drop();
            //    break;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Important Objects visible out of potential objects visible (including type), the number should be the same each time
        for(int i = 0; i < senses.visibleTargets.Count; ++i)
        {
            //3
            sensor.AddObservation(senses.visibleTargets[i].transform.localPosition);
            //1
            sensor.AddObservation(senses.visibleTargets[i].gameObject.layer);
            //Looping through the objects seen and adding the positions and what they are
        }

        for(int i = 0; i < senses.obstacles.Count; ++i)
        {
            //3
            sensor.AddObservation(senses.obstacles[i].transform.localPosition);
            //1
            sensor.AddObservation(senses.obstacles[i].gameObject.layer);
            //Looping through the objects seen and adding the positions and what they are
        }

        List<Vector3> hitPoints = senses.GetFOV();

        //Environment Objects within your FOV (the position amounts should be the same)
        for(int i = 0; i < hitPoints.Count; ++i)
        {
            //Looping through the points of the FOV
            // 3
            sensor.AddObservation(hitPoints[i]);
        }

        //Adding if the AI is holding a gun
        sensor.AddObservation(characterPickup.bEquipped);

        //Direction faced by this AI
        sensor.AddObservation(transform.forward);

        //Position of this AI
        sensor.AddObservation(transform.localPosition);

        //Health
        sensor.AddObservation(health.charHealth);

        //Environment Observations (Could be incomplete)
        for(int i = 0; i < instanceManager.bulletPoolMaxSize; ++i)
        {
            GameObject bullet = instanceManager.bulletPool.Get();
            //Looping through the bullets, and seeing where they are and which direction they're going and what speed (reserve space for all potential used size of object pools)
            if(bullet != null)
            {
                sensor.AddObservation(bullet.transform.position);

                if(bullet.TryGetComponent(out Bullet bulletScript))
                {
                    Rigidbody rb = bulletScript.GetRB();
                    if(rb != null)
                    {
                        sensor.AddObservation(rb.velocity.normalized);
                    }
                }
            }
        }
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
            AddReward(-5f);
        }
        //Round Win
        else if (endState == (int)ShooterGameOutcome.Win)
        {
            AddReward(25f);
        }
        //Round Lose
        else if(endState == (int)ShooterGameOutcome.Lose)
        {
            AddReward(-25f);
        }

        EndEpisode();
    }

    public void ResetStats()
    {
        health.ResetHealth();
    }

    #endregion
}
