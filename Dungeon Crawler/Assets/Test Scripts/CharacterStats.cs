using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int m_maxHealth = 100;
    public int m_currentHealth { get; private set; }

    public Stat m_damage;
    public Stat m_armour;

    private void Awake()
    {
        m_currentHealth = m_maxHealth;
    }

    private void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        damage -= m_armour.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        m_currentHealth -= damage;
        Debug.Log(transform.name + "takes " + damage + "damage.");

        if (m_currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Die in some way
        //Meant to be overriden
        Debug.Log(transform.name + "died.");
    }
}
