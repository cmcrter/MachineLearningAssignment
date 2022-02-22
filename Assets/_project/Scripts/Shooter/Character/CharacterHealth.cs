////////////////////////////////////////////////////////////
// File: CharacterHealth.cs
// Author: Charles Carter
// Date Created: 21/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 21/02/22
// Brief: A script to keep the health of the character
//////////////////////////////////////////////////////////// 

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
    }

    #endregion

    #region Variables

    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float fullHealth;

    public bool isDead = false;

    #endregion

    #region Unity Methods

    private void Start()
    {
        currentHealth = fullHealth;
    }

    #endregion

    #region Private Methods

    private bool isCharacterDead()
    {
        return (false ? currentHealth != 0 : true);
    }

    private void CallCharacterDeath()
    {
        //Update the UI on the character

        //Play the explosion

        //Wait before telling instance/game manager

    }

    #endregion
}
