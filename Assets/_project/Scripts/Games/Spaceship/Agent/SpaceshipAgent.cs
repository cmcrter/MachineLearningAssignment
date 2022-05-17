using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

/// <summary>
/// A hummingbird Machine Learning Agent
/// </summary>
public class SpaceshipAgent : Agent
{
    [Tooltip("Force to apply when moving")]
    public float moveForce = 2f;

    [Tooltip("Speed to pitch up or down")]
    public float pitchSpeed = 100f;

    [Tooltip("Speed to rotate around the up axis")]
    public float yawSpeed = 100f;

    [Tooltip("Transform at the tip of the beak")]
    public Transform beakTip;

    [Tooltip("The agent's camera")]
    public Camera agentCamera;

    [Tooltip("Whether this is training mode or gameplay mode")]
    public bool trainingMode;

    // The rigidbody of the agent
    [SerializeField]
    private Rigidbody rb;

    // The computers area that the agent is in
    public ComputerArea computersArea;

    // The nearest computers to the agent
    private Computer nearestcomputers;

    // Allows for smoother pitch changes
    private float smoothPitchChange = 0f;

    // Allows for smoother yaw changes
    private float smoothYawChange = 0f;

    // Maximum angle that the bird can pitch up or down
    private const float MaxPitchAngle = 100;

    // Maximum distance from the beak tip to accept Information collision
    private const float BeakTipRadius = 0.008f;

    // Whether the agent is frozen (intentionally not flying)
    private bool frozen = false;

    //The layer that the Computers are on
    [SerializeField]
    private LayerMask computersLayer;

    /// <summary>
    /// The amount of Information the agent has obtained this episode
    /// </summary>
    public float InformationObtained { get; private set; }

    /// <summary>
    /// Initialize the agent
    /// </summary>
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        computersArea = computersArea ?? FindObjectOfType<ComputerArea>();

        // If not training mode, no max step, play forever
        if (!trainingMode) MaxStep = 0;
    }

    /// <summary>
    /// Reset the agent when an episode begins
    /// </summary>
    public override void OnEpisodeBegin()
    {
        if (trainingMode && computersArea)
        {
            // Only reset Computers in training when there is one agent per area
            computersArea.ResetComputers();
        }

        // Reset Information obtained
        InformationObtained = 0f;

        // Zero out velocities so that movement stops before a new episode begins
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Default to spawning in front of a computers
        bool inFrontOfcomputers = true;
        if (trainingMode)
        {
            // Spawn in front of computers 50% of the time during training
            inFrontOfcomputers = UnityEngine.Random.value > .5f;
        }

        // Move the agent to a new random position
        MoveToSafeRandomPosition(inFrontOfcomputers);

        // Recalculate the nearest computers now that the agent has moved
        UpdateNearestcomputers();
    }

    /// <summary>
    /// Called when and action is received from either the player input or the neural network
    /// 
    /// vectorAction[i] represents:
    /// Index 0: move vector x (+1 = right, -1 = left)
    /// Index 1: move vector y (+1 = up, -1 = down)
    /// Index 2: move vector z (+1 = forward, -1 = backward)
    /// Index 3: pitch angle (+1 = pitch up, -1 = pitch down)
    /// Index 4: yaw angle (+1 = turn right, -1 = turn left)
    /// </summary>
    /// <param name="vectorAction">The actions to take</param>
    public override void OnActionReceived(ActionBuffers vectorAction)
    {
        // Don't take actions if frozen
        if (frozen) return;

        // Calculate movement vector
        Vector3 move = new Vector3(vectorAction.ContinuousActions[0], vectorAction.ContinuousActions[1], vectorAction.ContinuousActions[2]);

        // Add force in the direction of the move vector
        rb.AddForce(move * moveForce);

        // Get the current rotation
        Vector3 rotationVector = transform.rotation.eulerAngles;

        // Calculate pitch and yaw rotation
        float pitchChange = vectorAction.ContinuousActions[3];
        float yawChange = vectorAction.ContinuousActions[4];

        // Calculate smooth rotation changes
        smoothPitchChange = Mathf.MoveTowards(smoothPitchChange, pitchChange, 2f * Time.fixedDeltaTime);
        smoothYawChange = Mathf.MoveTowards(smoothYawChange, yawChange, 2f * Time.fixedDeltaTime);

        // Calculate new pitch and yaw based on smoothed values
        // Clamp  pitch to avoid flipping upside down
        float pitch = rotationVector.x + smoothPitchChange * Time.fixedDeltaTime * pitchSpeed;
        if (pitch > 180f) pitch -= 360f;
        pitch = Mathf.Clamp(pitch, -MaxPitchAngle, MaxPitchAngle);

        float yaw = rotationVector.y + smoothYawChange * Time.fixedDeltaTime * yawSpeed;

        // Apply the new rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    /// <summary>
    /// Collect vector observations from the environment
    /// </summary>
    /// <param name="sensor">The vector sensor</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        // If nearestcomputers is null, observe an empty array and return early
        if (nearestcomputers == null)
        {
            sensor.AddObservation(new float[10]);
            return;
        }

        // Observe the agent's local rotation (4 observations)
        sensor.AddObservation(transform.localRotation.normalized);

        // Get a vector from the beak tip to the nearest computers
        Vector3 tocomputers = nearestcomputers.computersCenterPosition - beakTip.position;

        // Observe a normalized vector pointing to the nearest computers (3 observations)
        sensor.AddObservation(tocomputers.normalized);

        // Observe a dot product that indicates whether the beak tip is in front of the computers (1 observation)
        // (+1 means that the beak tip is directly in front of the computers, -1 means directly behind)
        sensor.AddObservation(Vector3.Dot(tocomputers.normalized, -nearestcomputers.computersUpVector.normalized));

        // Observe a dot product that indicates whether the beak is pointing toward the computers (1 observation)
        // (+1 means that the beak is pointing directly at the computers, -1 means directly away)
        sensor.AddObservation(Vector3.Dot(beakTip.forward.normalized, -nearestcomputers.computersUpVector.normalized));

        // Observe the relative distance from the beak tip to the computers (1 observation)
        sensor.AddObservation(tocomputers.magnitude / ComputerArea.AreaDiameter);

        // 10 total observations
    }

    /// <summary>
    /// When Behavior Type is set to "Heuristic Only" on the agent's Behavior Parameters,
    /// this function will be called. Its return values will be fed into
    /// <see cref="OnActionReceived(ActionBuffers actionsOut)"/> instead of using the neural network
    /// </summary>
    /// <param name="actionsOut">And output action array</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        // Create placeholders for all movement/turning
        Vector3 forward = Vector3.zero;
        Vector3 left = Vector3.zero;
        Vector3 up = Vector3.zero;
        float pitch = 0f;
        float yaw = 0f;

        // Convert keyboard inputs to movement and turning
        // All values should be between -1 and +1

        // Forward/backward
        if (Input.GetKey(KeyCode.W)) forward = transform.forward;
        else if (Input.GetKey(KeyCode.S)) forward = -transform.forward;

        // Left/right
        if (Input.GetKey(KeyCode.A)) left = -transform.right;
        else if (Input.GetKey(KeyCode.D)) left = transform.right;

        // Up/down
        if (Input.GetKey(KeyCode.E)) up = transform.up;
        else if (Input.GetKey(KeyCode.C)) up = -transform.up;

        // Pitch up/down
        if (Input.GetKey(KeyCode.UpArrow)) pitch = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) pitch = -1f;

        // Turn left/right
        if (Input.GetKey(KeyCode.LeftArrow)) yaw = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) yaw = 1f;

        // Combine the movement vectors and normalize
        Vector3 combined = (forward + left + up).normalized;

        // Add the 3 movement values, pitch, and yaw to the actionsOut array
        continuousActions[0] = combined.x;
        continuousActions[1] = combined.y;
        continuousActions[2] = combined.z;
        continuousActions[3] = pitch;
        continuousActions[4] = yaw;   
    }

    /// <summary>
    /// Prevent the agent from moving and taking actions
    /// </summary>
    public void FreezeAgent()
    {
        //Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = true;
        rb.Sleep();
    }

    /// <summary>
    /// Resume agent movement and actions
    /// </summary>
    public void UnfreezeAgent()
    {
       // Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = false;
        rb.WakeUp();
    }

    /// <summary>
    /// Move the agent to a safe random position (i.e. does not collide with anything)
    /// If in front of computers, also point the beak at the computers
    /// </summary>
    /// <param name="inFrontOfcomputers">Whether to choose a spot in front of a computers</param>
    private void MoveToSafeRandomPosition(bool inFrontOfcomputers)
    {
        bool safePositionFound = false;
        int attemptsRemaining = 100; // Prevent an infinite loop
        Vector3 potentialPosition = Vector3.zero;
        Quaternion potentialRotation = new Quaternion();

        // Loop until a safe position is found or we run out of attempts
        while (!safePositionFound && attemptsRemaining > 0)
        {
            attemptsRemaining--;
            if (inFrontOfcomputers)
            {
                // Pick a random computers
                Computer randomcomputers = computersArea.Computers[UnityEngine.Random.Range(0, computersArea.Computers.Count)];

                // Position 10 to 20 cm in front of the computers
                float distanceFromcomputers = UnityEngine.Random.Range(.1f, .2f);
                potentialPosition = randomcomputers.transform.position + randomcomputers.computersUpVector * distanceFromcomputers;

                // Point beak at computers (bird's head is center of transform)
                Vector3 tocomputers = randomcomputers.computersCenterPosition - potentialPosition;
                potentialRotation = Quaternion.LookRotation(tocomputers, Vector3.up);
            }
            else
            {
                // Pick a random height from the ground
                float height = UnityEngine.Random.Range(20f, 32.5f);

                // Pick a random radius from the center of the area
                float radius = UnityEngine.Random.Range(1f, 127f);

                // Pick a random direction rotated around the y axis
                Quaternion direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 180f), 0f);

                // Combine height, radius, and direction to pick a potential position
                potentialPosition = computersArea.transform.position + Vector3.up * height + direction * Vector3.forward * radius;

                // Choose and set random starting pitch and yaw
                float pitch = UnityEngine.Random.Range(-60f, 60f);
                float yaw = UnityEngine.Random.Range(-180f, 180f);
                potentialRotation = Quaternion.Euler(pitch, yaw, 0f);
            }

            // Check to see if the agent will collide with anything
            Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.02f, ~computersLayer);

            // Safe position has been found if no colliders are overlapped
            safePositionFound = colliders.Length == 0;
        }

        Debug.Assert(safePositionFound, "Could not find a safe position to spawn");

        //Keep the position and rotation as the scene values
        transform.position = potentialPosition;
        transform.rotation = potentialRotation;
    }

    /// <summary>
    /// Update the nearest computers to the agent
    /// </summary>
    private void UpdateNearestcomputers()
    {
        foreach (Computer computers in computersArea.Computers)
        {
            if (nearestcomputers == null && computers.HasInformation)
            {
                // No current nearest computers and this computers has Information, so set to this computers
                nearestcomputers = computers;
            }
            else if (computers.HasInformation)
            {
                // Calculate distance to this computers and distance to the current nearest computers
                float distanceTocomputers = Vector3.Distance(computers.transform.position, beakTip.position);
                float distanceToCurrentNearestcomputers = Vector3.Distance(nearestcomputers.transform.position, beakTip.position);

                // If current nearest computers is empty OR this computers is closer, update the nearest computers
                if (!nearestcomputers.HasInformation || distanceTocomputers < distanceToCurrentNearestcomputers)
                {
                    nearestcomputers = computers;
                }
            }
        }
    }

    /// <summary>
    /// Called when the agent's collider enters a trigger collider
    /// </summary>
    /// <param name="other">The trigger collider</param>
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterOrStay(other);
    }

    /// <summary>
    /// Called when the agent's collider stays in a trigger collider
    /// </summary>
    /// <param name="other">The trigger collider</param>
    private void OnTriggerStay(Collider other)
    {
        TriggerEnterOrStay(other);
    }

    /// <summary>
    /// Handles when the agen'ts collider enters or stays in a trigger collider
    /// </summary>
    /// <param name="collider">The trigger collider</param>
    private void TriggerEnterOrStay(Collider collider)
    {
        // Check if agent is colliding with Information
        if (collider.gameObject.layer == 7)
        {
            Vector3 closestPointToBeakTip = collider.ClosestPoint(beakTip.position);

            // Check if the closest collision point is close to the beak tip
            // Note: a collision with anything but the beak tip should not count
            if (Vector3.Distance(beakTip.position, closestPointToBeakTip) < BeakTipRadius)
            {
                // Look up the computers for this Information collider
                Computer computers = computersArea.GetcomputersFromInformation(collider);

                // Attempt to take .01 Information
                // Note: this is per fixed timestep, meaning it happens every .02 seconds, or 50x per second
                float InformationReceived = computers.Feed(.01f);

                // Keep track of Information obtained
                InformationObtained += InformationReceived;

                if (trainingMode)
                {
                    // Calculate reward for getting Information
                    float bonus = .02f * Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, -nearestcomputers.computersUpVector.normalized));
                    AddReward(.01f + bonus);
                }

                // If computers is empty, update the nearest computers
                if (!computers.HasInformation)
                {
                    UpdateNearestcomputers();
                }
            }
        }
    }

    /// <summary>
    /// Called when the agent collides with something solid
    /// </summary>
    /// <param name="collision">The collision info</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (trainingMode && collision.collider.CompareTag("Boundary"))
        {
            // Collided with the area boundary, give a negative reward
            AddReward(-.5f);
        }
    }

    private void Awake()
    {
        rb = rb ?? GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update()
    {
        // Draw a line from the beak tip to the nearest computers
        if (nearestcomputers != null)
            Debug.DrawLine(beakTip.position, nearestcomputers.computersCenterPosition, Color.green);
    }

    /// <summary>
    /// Called every .02 seconds
    /// </summary>
    private void FixedUpdate()
    {
        // Avoids scenario where nearest computers Information is stolen by opponent and not updated
        if (nearestcomputers != null && !nearestcomputers.HasInformation)
            UpdateNearestcomputers();
    }
}
