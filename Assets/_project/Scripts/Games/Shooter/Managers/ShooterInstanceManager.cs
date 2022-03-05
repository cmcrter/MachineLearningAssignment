////////////////////////////////////////////////////////////
// File: ShooterInstanceManager.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: A script for AI to train in a simulated game
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//Not a singleton due to multiple instances speeding up training
public class ShooterInstanceManager : MonoBehaviour
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
    private List<MLInputManager> Agents;
    private CharacterInputManager Player;

    [Header("Round Timer Variables")]
    [SerializeField]
    private float RoundDuration = 300f;
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

        //Going through the guns already on the level and setting them up
        foreach(Gun gun in guns)
        {
            gun.InstialiseGun(bullets);
        }

        roundTime = new Timer(RoundDuration);
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

    //If there's no bullet pool, this will create one
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
        //A loop of the round timer to play whilst the game is played
        UpdateUI();
        yield return new WaitForSeconds(1.0f);

        while(roundTime.isActive)
        {
            roundTime.Tick(Time.deltaTime);
            UpdateUI();

            yield return null;
        }

        RoundOver();
        roundCoroutine = null;
    }

    private void UpdateUI()
    {
        if(RoundTimerText == null) return;

        //Getting the closest second
        int seconds = Mathf.FloorToInt(roundTime.current_time % 60);
        int minutes = Mathf.FloorToInt(roundTime.current_time / 60);

        //If it's above 0 display it as a second
        if(seconds > 0 || minutes > 0)
        {
            RoundTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    private void RoundOver()
    {
        
    }

    //A player has died, checking whether this round is over
    private void CheckRoundOverCondition()
    {
    
    }

    #endregion
}
