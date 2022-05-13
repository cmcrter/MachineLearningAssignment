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

    public static ShooterInstanceManager instance;

    [SerializeField]
    private TrainingType currentTraining;

    [SerializeField]
    private List<MLShooter> Agents;
    private CharacterInputManager Player;

    [SerializeField]
    private List<Transform> startPoints;

    [SerializeField]
    private List<CharacterActions> pickups;

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
    public int bulletPoolMaxSize = 50;
    [SerializeField]
    private List<Gun> guns;
    public bool collectionChecks = true;
    [SerializeField]
    Transform bulletParent;

    Coroutine peaceCoroutine;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        bullets = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, bulletPoolMaxSize);
    }

    private void Start()
    {
        CallRestartInstance();
    }

    #endregion

    #region Public Methods

    public float GetDirBetweenAgents(MLShooter shootingAgent)
    {
        List<Vector3> directions = new List<Vector3>();
        float score = 0;

        foreach(MLShooter shooter in Agents)
        {
            if(shooter == shootingAgent || !shootingAgent.senses.obstacles.Contains(shooter.transform))
            {
                continue;
            }

            if(shooter.gameObject.layer != shootingAgent.gameObject.layer)
            {
                directions.Add((shooter.transform.position - shootingAgent.transform.position).normalized);
            }
        }

        foreach(Vector3 direction in directions)
        {
            score += Vector3.Dot(direction, shootingAgent.transform.forward);
        }

        return score;
    }

    public void CallRestartInstance()
    {
        for(int i = 0; i < Agents.Count; ++i)
        {
            Agents[i].characterActions.movementController.enabled = false;
            Agents[i].transform.position = startPoints[i].transform.position;
            Agents[i].transform.rotation = startPoints[i].transform.rotation;
            Agents[i].ResetStats();
            Agents[i].characterActions.movementController.enabled = true;
            Agents[i].OnEpisodeBegin();
        }

        foreach(CharacterActions pickup in pickups)
        {
            pickup.Drop();
        }

        foreach(Gun gun in guns)
        {
            gun.ResetGun();
        }

        if(roundCoroutine != null)
        {
            roundCoroutine = null;
        }

        peaceCoroutine = null;

        roundCoroutine = StartCoroutine(Co_RoundTimer());
        peaceCoroutine = StartCoroutine(Co_PeaceStopper());
    }

    public void CharacterDied(MLShooter shooterThatDied)
    {
        foreach(MLShooter shooter in Agents)
        {
            if(shooter == shooterThatDied)
            {
                shooter.CollectReward((int)ShooterGameOutcome.Lose);
            }
            else
            {
                shooter.CollectReward((int)ShooterGameOutcome.Win);
            }
        }

        CallRestartInstance();
    }

    public void RewardShooter(MLShooter shooter)
    {
        if(shooter)
        {
            shooter.AddReward(5f);
        }
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
        roundTime = new Timer(RoundDuration);

        //A loop of the round timer to play whilst the game is played
        while(roundTime.isActive)
        {
            roundTime.Tick(Time.deltaTime);
            UpdateUI();

            yield return null;
        }

        RoundOver();
        roundCoroutine = null;
    }

    private IEnumerator Co_PeaceStopper()
    {
        while(roundCoroutine != null)
        {
            yield return new WaitForSeconds(10f);

            foreach(MLShooter shooter in Agents)
            {
                if(shooter.bShotSomething)
                {
                    shooter.AddReward(1f);
                }
                else
                {
                    shooter.AddReward(-1f);
                }

                shooter.bShotSomething = false;
            }
        }
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
        float difference = Agents[0].health.charHealth - Agents[1].health.charHealth;
        float rewardDiff = (difference / 10f);
    
        if(difference > 0)
        {
            Agents[0].AddReward(rewardDiff);
            Agents[1].AddReward(rewardDiff * -1f);
        }
        else if(Agents[1].health.charHealth > Agents[0].health.charHealth)
        {
            Agents[1].AddReward(rewardDiff);
            Agents[0].AddReward(rewardDiff * -1f);
        }
        else 
        {
            Agents[1].CollectReward((int)ShooterGameOutcome.Draw);
            Agents[0].CollectReward((int)ShooterGameOutcome.Draw);
        }

        foreach(MLShooter shooter in Agents)
        {

            shooter.EndEpisode();
        }

        CallRestartInstance();
    }

    //A player has died, checking whether this round is over
    private void CheckRoundOverCondition()
    {
        
    }

    #endregion
}
