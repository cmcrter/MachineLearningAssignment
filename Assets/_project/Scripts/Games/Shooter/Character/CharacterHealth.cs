////////////////////////////////////////////////////////////
// File: CharacterHealth.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/02/22
// Brief: A script to keep the health of the character
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    #region Interface Contracts

    GameObject IDamageable.gameObject => gameObject;
    bool IDamageable.canDamage => gameObject.activeInHierarchy;

    float IDamageable.MaxHealth
    {
        get => fullHealth;
        set => fullHealth = value;
    }
    float IDamageable.Health
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    void IDamageable.Damaged(float damage)
    {
        if(isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, fullHealth);

        if(shooter)
        {
            shooter.AddReward(-1f);
        }

        UpdateUI(currentHealth / fullHealth);

        if(isCharacterDead())
        {
            isDead = true;
            CallCharacterDeath();
        }
    }

    void IDamageable.Healed(float damage)
    {
        if(isDead)
            return;

        currentHealth += damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, fullHealth);

        UpdateUI(currentHealth / fullHealth);
    }

    #endregion

    #region Variables

    [SerializeField]
    ShooterInstanceManager instanceManager;

    [SerializeField]
    CharacterInputManager mainCharacterManager;

    [SerializeField]
    MLShooter shooter;

    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float fullHealth;

    public bool isDead = false;

    public Transform HealthVisualiser;
	public Transform healthBar;
	public Transform backgroundBar;
	public bool showWhenFull;
    [SerializeField]
	private Transform CameraToFace;

    public float charHealth
    {
        get => currentHealth;
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        currentHealth = fullHealth;
        UpdateUI(1.0f);
    }

    private void Update()
    {
        Vector3 direction = CameraToFace.transform.forward;
        HealthVisualiser.transform.forward = -direction;
    }

    #endregion

    #region Private Methods

    public void ResetHealth()
    {
        currentHealth = fullHealth;
        UpdateUI(1f);
    }

    public void UpdateUI(float normalizedHealth)
    {
        Vector3 scale = Vector3.one;

        if(healthBar != null)
        {
            scale.x = normalizedHealth;
            healthBar.transform.localScale = scale;
        }

        if(backgroundBar != null)
        {
            scale.x = 1 - normalizedHealth;
            backgroundBar.transform.localScale = scale;
        }

        SetVisible(showWhenFull || normalizedHealth < 1.0f);
    }

    public void SetVisible(bool visible)
    {
        HealthVisualiser.gameObject.SetActive(visible);
    }

    private bool isCharacterDead()
    {
        return (currentHealth == 0);
    }

    private void CallCharacterDeath()
    {
        if(Debug.isDebugBuild)
        {
            Debug.Log("Character Dead: " + transform.name, this);
        }

        //Play VFX

        //Wait before telling instance/game manager
        StartCoroutine(Co_DeathCooldown());
    }

    private IEnumerator Co_DeathCooldown()
    {
        yield return new WaitForSeconds(0.5f);

        if(instanceManager && shooter)
        {
            instanceManager.CharacterDied(shooter);
        }
    }

    #endregion
}
