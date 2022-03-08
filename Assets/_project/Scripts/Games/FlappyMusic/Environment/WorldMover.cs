////////////////////////////////////////////////////////////
// File: WorldMover.cs
// Author: Charles Carter
// Date Created: 08/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 08/03/22
// Brief: A script to move the world, simulating character movement
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WorldMover : MonoBehaviour
{
    #region Public Fields

    [Header("Necessary Variables")]
    [SerializeField]
    private Transform worldTransform;

    [SerializeField]
    private GameObject pipePrefab;
    ObjectPool<PipeSet> PipeSetPool;

    [SerializeField]
    private BirdMovement player;
    [SerializeField]
    private MLFlappyAgent agent;

    //This allows me to know the order of the pipes that are active
    private List<PipeSet> currentlyActivePipes = new List<PipeSet>();

    [Header("Customization Variables")]
    [SerializeField]
    private float movementSpeed = 3f;
    [SerializeField]
    private float pipeDistance = 8f;

    [SerializeField]
    private float startingPipes = 3f;
    private int pipePoolMaxSize = 10;

    [SerializeField]
    private int currentScore = 0;
    [SerializeField]
    private TextMesh scoreText;

    public bool collectionChecks = true;

    public bool gameActive = true;

    #endregion

    #region Unity Methods

    void Start()
    {
        SetInitialPipes();

        if(agent)
        {
            agent.OnEpisodeBegin();
        }
    }

    void Update()
    {
        if(!gameActive)
            return;

        for(int i = 0; i < worldTransform.childCount; ++i)
        {
            worldTransform.GetChild(i).transform.position -= new Vector3(0, 0, movementSpeed * Time.deltaTime);
        }

        //Stopping the AI from tapping so quickly, that it has a greater force than the collision detection
        if(player.transform.localPosition.y > 8 || player.transform.localPosition.y < -8)
        {
            agent.AddReward(-5f);
            PlayerDied();
        }
    }

    #endregion

    #region Pipe Pool Methods

    //If there's no pool, this will create one
    public ObjectPool<PipeSet> pipeSetPool
    {
        get
        {
            if(PipeSetPool == null)
            {
                PipeSetPool = new ObjectPool<PipeSet>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, pipePoolMaxSize);
            }

            return PipeSetPool;
        }
    }

    private PipeSet CreatePooledItem()
    {
        GameObject go = Instantiate(pipePrefab, worldTransform);
        return go.GetComponent<PipeSet>();
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(PipeSet go)
    {
        go.gameObject.SetActive(false);
        currentlyActivePipes.Remove(go);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(PipeSet go)
    {
        go.gameObject.SetActive(true);
        go.Initialise(this);

        if(currentlyActivePipes.Count > 0)
        {
            go.SetPosZ(currentlyActivePipes[currentlyActivePipes.Count - 1].transform.position.z + pipeDistance);
        }

        currentlyActivePipes.Add(go);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(PipeSet go)
    {
        Destroy(go.gameObject);
    }

    #endregion

    #region Public Methods

    public void PipeHasReachedTheEnd(PipeSet pipe)
    {
        PipeSetPool.Release(pipe);
        PipeSetPool.Get();
    }

    public void PlayerDied()
    {        
        if(Debug.isDebugBuild)
        {
           // Debug.Log("Player Died");
        }

        if(agent)
        {
            if(agent.enabled)
            {
                agent.AddReward(-1f);
                agent.EndEpisode();
            }
        }

        GameComplete();
    }

    public void GameComplete()
    {
        gameActive = false;

        //Clearing the collections
        pipeSetPool.Dispose();
        currentlyActivePipes.Clear();

        //Disposing in the object pool doesn't remove the objects
        for(int i = 0; i < worldTransform.childCount; ++i)
        {
            Destroy(worldTransform.GetChild(i).gameObject);       
        }

        StartCoroutine(Co_GameOver());
    }

    public void AddScore()
    {
        currentScore++;

        if(scoreText)
        {
            scoreText.text = currentScore.ToString();
        }

        if(agent)
        {
            if(agent.enabled)
            {
                agent.AddReward(5f);
            }  
        }
    }
    public Transform GetNextPipe()
    {
        float leftMost = float.MaxValue;
        Transform leftChild = null;

        foreach(Transform child in transform)
        {
            float childZ = child.localPosition.z;
            if(childZ < leftMost && childZ > -.3f)
            {
                leftChild = child;
                leftMost = child.localPosition.z;
            }
        }

        return leftChild;
    }

    #endregion

    #region Private Methods

    private IEnumerator Co_GameOver()
    {
        yield return new WaitForSeconds(1.0f);
        ResetWorld();
        player.ResetPlayer();
    }

    private void ResetWorld()
    {
        currentScore = 0;

        if(scoreText)
        {
            scoreText.text = currentScore.ToString();
        }

        SetInitialPipes();

        if(agent)
        {
            if(agent.enabled)
            {
                agent.OnEpisodeBegin();
            }
        }

        gameActive = true;
    }

    private void SetInitialPipes()
    {
        for(int i = 0; i < startingPipes; ++i)
        {
            PipeSet pipe = pipeSetPool.Get();
            pipe.transform.position = new Vector3(transform.position.x, pipe.transform.position.y, i * pipeDistance);
        }
    }

    #endregion
}
