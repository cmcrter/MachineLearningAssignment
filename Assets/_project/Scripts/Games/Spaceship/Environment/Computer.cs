using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a single computers with Information
/// </summary>
public class Computer : MonoBehaviour
{
    [Tooltip("The color when the computers is full")]
    public Color fullcomputersColor = new Color(1f, 0f, .3f);

    [Tooltip("The color when the computers is empty")]
    public Color emptycomputersColor = new Color(.5f, 0f, 1f);

    /// <summary>
    /// The trigger collider representing the Information
    /// </summary>
    public Collider InformationCollider;

    // The solid collider representing the computers
    public Collider computersCollider;

    // The computers's material
    private Material computersMaterial;

    /// <summary>
    /// A vector pointing straight out of the computers
    /// </summary>
    public Vector3 computersUpVector
    {
        get
        {
            return InformationCollider.transform.up;
        }
    }

    /// <summary>
    /// The center position of the Information collider
    /// </summary>
    public Vector3 computersCenterPosition
    {
        get
        {
            return InformationCollider.transform.position;
        }
    }

    /// <summary>
    /// The amount of Information remaining in the computers
    /// </summary>
    public float InformationAmount { get; private set; }

    /// <summary>
    /// Whether the computers has any Information remaining
    /// </summary>
    public bool HasInformation
    {
        get
        {
            return InformationAmount > 0f;
        }
    }

    /// <summary>
    /// Attempts to remove Information from the computers
    /// </summary>
    /// <param name="amount">The amount of Information to remove</param>
    /// <returns>The actual amount successfully removed</returns>
    public float Feed(float amount)
    {
        // Track how much Information was successfully taken (cannot take more than is available)
        float InformationTaken = Mathf.Clamp(amount, 0f, InformationAmount);

        // Subtract the Information
        InformationAmount -= amount;

        if (InformationAmount <= 0)
        {
            // No Information remaining
            InformationAmount = 0;

            // Disable the computers and Information colliders
            computersCollider.gameObject.SetActive(false);
            InformationCollider.gameObject.SetActive(false);

            // Change the computers color to indicate that it is empty
            computersMaterial.SetColor("_BaseColor", emptycomputersColor);
        }

        // Return the amount of Information that was taken
        return InformationTaken;
    }

    /// <summary>
    /// Resets the computers
    /// </summary>
    public void Resetcomputers()
    {
        // Refill the Information
        InformationAmount = 1f;

        // Enable the computers and Information colliders
        computersCollider.gameObject.SetActive(true);
        InformationCollider.gameObject.SetActive(true);

        // Change the computers color to indicate that it is full
        computersMaterial.SetColor("_BaseColor", fullcomputersColor);
    }

    /// <summary>
    /// Called when the computers wakes up
    /// </summary>
    private void Awake()
    {
        // Find the computers's mesh renderer and get the main material
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        computersMaterial = meshRenderer.material;

        // Find computers and Information colliders
        computersCollider = computersCollider ?? GetComponent<Collider>();
        InformationCollider = InformationCollider ?? transform.Find("Information").GetComponent<Collider>();
    }
}
