using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public override void ActivateEffect()
    {
        CharacterStats myStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        myStats.m_currentHealth = Mathf.Clamp(myStats.m_currentHealth + (int)m_potency, 0, myStats.m_maxHealth);
    }

}
