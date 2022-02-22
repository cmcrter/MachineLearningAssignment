////////////////////////////////////////////////////////////
// File: InstanceManager.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: A script for AI to train in a simulated game
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//Not a singleton due to multiple instances speeding up training
public class InstanceManager : MonoBehaviour
{
    public enum TrainingType
    {
        vsPlayer = 0,
        AimPractice = 1,
        vsSelf
    }

    #region Variables

    [SerializeField]
    private TrainingType currentTraining;

    [SerializeField]
    private List<IAgentable> Agents;

    [SerializeField]
    private Timer roundTime;
    private Coroutine roundCoroutine;
    [SerializeField]
    private TextMesh RoundTimerText;

    [Header("Bullet Object Pool Setup")]
    [SerializeField]
    private GameObject bulletPrefab;
    public IObjectPool<GameObject> bullets;
    [SerializeField]
    private int bulletPoolMaxSize = 50;
    [SerializeField]
    private List<Gun> guns;
    public bool collectionChecks = true;
    [SerializeField]
    Transform bulletParent;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        bullets = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, bulletPoolMaxSize);

        foreach(Gun gun in guns)
        {
            gun.InstialiseGun(bullets);
        }

        roundTime = new Timer(600);
    }

    private void OnEnable()
    {
        if(roundCoroutine != null)
        {
            roundCoroutine = null;
        }

        roundCoroutine = StartCoroutine(Co_RoundTimer());
    }

    #endregion

    #region Private Methods

    public IObjectPool<GameObject> bulletPool
    {
        get
        {
            if(bullets == null)
            {
                bullets = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, bulletPoolMaxSize);
            }

            return bullets;
        }
    }

    private GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(bulletPrefab, bulletParent);
        return go;
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(GameObject go)
    {
        go.layer = 0;
        go.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }

    private IEnumerator Co_RoundTimer()
    {
        while(roundTime.isActive)
        {
            roundTime.Tick(Time.deltaTime);

            if(RoundTimerText)
            {
                RoundTimerText.text = roundTime.current_time.ToString();
            }

            yield return null;
        }

        roundCoroutine = null;
    }

    #endregion
}
