using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a collection of computers plants and attached Computers
/// </summary>
public class ComputerArea : MonoBehaviour
{
    // The diameter of the area where the agent and Computers can be
    // used for observing relative distance from agent to computers
    public const float AreaDiameter = 150;

    // The list of all computers plants in this computers area (computers plants have multiple Computers)
    private List<GameObject> computersPlants;

    // A lookup dictionary for looking up a computers from a Information collider
    private Dictionary<Collider, Computer> InformationcomputersDictionary;

    /// <summary>
    /// The list of all Computers in the computers area
    /// </summary>
    public List<Computer> Computers { get; private set; }

    /// <summary>
    /// Procedurally putting down this many Computers in the centre boundary box
    /// </summary>
    [SerializeField]
    private int RandomComputers;
    [SerializeField]
    private BoxCollider colliderToPutRandComputers;

    [SerializeField]
    private GameObject computerPrefab;

    /// <summary>
    /// Reset the Computers and computers plants
    /// </summary>
    public void ResetComputers()
    {
        // Rotate each computers plant around the Y axis and subtly around X and Z
        foreach (GameObject computersPlant in computersPlants)
        {
            //float xRotation = UnityEngine.Random.Range(-5f, 5f);
            float yRotation = UnityEngine.Random.Range(-180f, 180f);
            //float zRotation = UnityEngine.Random.Range(-5f, 5f);
            computersPlant.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        }

        // Reset each computers
        foreach (Computer computers in Computers)
        {
            computers.Resetcomputers();
        }
    }

    /// <summary>
    /// Gets the <see cref="computers"/> that a Information collider belongs to
    /// </summary>
    /// <param name="collider">The Information collider</param>
    /// <returns>The matching computers</returns>
    public Computer GetcomputersFromInformation(Collider collider)
    {
        return InformationcomputersDictionary[collider];
    }

    /// <summary>
    /// Called when the area wakes up
    /// </summary>
    private void Awake()
    {
        // Initialize variables
        computersPlants = new List<GameObject>();
        InformationcomputersDictionary = new Dictionary<Collider, Computer>();
        Computers = new List<Computer>();

        PlacingRandomComputers();
    }

    /// <summary>
    /// Called when the game starts
    /// </summary>
    private void Start()
    {
        // Find all Computers that are children of this GameObject/Transform
        FindChildComputers(transform);
    }

    /// <summary>
    /// Recursively finds all Computers and computers plants that are children of a parent transform
    /// </summary>
    /// <param name="parent">The parent of the children to check</param>
    private void FindChildComputers(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.TryGetComponent(out Computer computers))
            {
                // Found a computers, add it to the Computers list
                Computers.Add(computers);

                // Add the Information collider to the lookup dictionary
                InformationcomputersDictionary.Add(computers.InformationCollider, computers);

                // Note: there are no Computers that are children of other Computers
            }
            else
            {
                // Look for Computers within the computers plant
                FindChildComputers(child);
            }
        }
    }

    private void PlacingRandomComputers()
    {
        for(int i = 0; i < RandomComputers; ++i)
        {
            float randX = Random.Range(colliderToPutRandComputers.bounds.min.x, colliderToPutRandComputers.bounds.max.x);
            float randZ = Random.Range(colliderToPutRandComputers.bounds.min.z, colliderToPutRandComputers.bounds.max.z);

            Instantiate(computerPrefab, new Vector3(randX, colliderToPutRandComputers.bounds.min.y, randZ), Quaternion.identity, transform);
        }
    }
}
