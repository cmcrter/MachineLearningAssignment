////////////////////////////////////////////////////////////
// File: PipeSet.cs
// Author: Charles Carter
// Date Created: 08/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 08/03/22
// Brief: A script to object pool the pipes
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class PipeSet : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float randomHeightVariance;

    [SerializeField]
    private PipeTrigger pipeTrigger;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        pipeTrigger = pipeTrigger ?? GetComponentInChildren<PipeTrigger>();
    }

    private void OnEnable()
    {
        float randHeight = Random.Range(-randomHeightVariance, randomHeightVariance);
        transform.position = new Vector3(transform.position.x, transform.position.y + randHeight, transform.position.z);
    }

    void OnDisable()
    {
        
    }

    #endregion

    #region Public Methods

    public void Initialise(WorldMover newManager)
    {
        pipeTrigger.manager = newManager;
    }

    public void SetPosZ(float newPos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
    }

    #endregion
}
