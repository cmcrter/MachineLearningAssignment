////////////////////////////////////////////////////////////
// File: MLDriver.cs
// Author: Charles Carter
// Date Created: 21/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/03/22
// Brief: The driver controlled by Unity ML
//////////////////////////////////////////////////////////// 

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLDriver : Agent /*, IAgentable*/
{
    #region Variables

    public int driverID = 0;

    public int checkpointNum = 0;
    public int lapCount = 0;

    [SerializeField]
    private RacecarSensors sensors;

    [SerializeField]
    private RacecarMovement movement;

    #endregion
 
    #region Unity Methods

    void Start()
    {
	
    }
 
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            Debug.Log("Collided:" + contact.otherCollider.name);
            AddReward(-0.2f);
        }
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
        if(Debug.isDebugBuild)
        {
            //Debug.Log("Accelerate: " + actions.DiscreteActions[0]);
            //Debug.Log("Brake: " + actions.DiscreteActions[1]);
            //Debug.Log("Steer Left: " + actions.DiscreteActions[2]);
            //Debug.Log("Steer Right: " + actions.DiscreteActions[3]);
        }

        //Accelerate
        if(actions.DiscreteActions[0] == 1)
        {
            movement.Accerlerate();
        }

        //Brake
        if(actions.DiscreteActions[1] == 1)
        {
            movement.Brake();
        }

        //Steer Left
        if(actions.DiscreteActions[2] == 1)
        {
            movement.SteerLeft();
        }

        //Steer Right
        if(actions.DiscreteActions[3] == 1)
        {
            movement.SteerRight();
        }

        actions.Clear();

        if(MaxStep > 0)
            AddReward(-1f / MaxStep);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float[] rcasts = sensors.returnNormalisedDistances();

        for(int i = 0; i < rcasts.Length; ++i)
        {
            // 10 values
            sensor.AddObservation(rcasts[i]);
        }

        // 3 values 
        sensor.AddObservation(transform.position);

        // 3 values 
        sensor.AddObservation(transform.forward);

        // 3 values 
        sensor.AddObservation(movement.rb.velocity);

        base.CollectObservations(sensor);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {

    }

    public void LapFinished()
    {
        EndEpisode();
    }

    #endregion

    #region Private Methods
    #endregion
}