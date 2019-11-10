﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    
    
    /// <summary>
    /// Awake is called when a script is  loaded for the first time.
    /// </summary>
    private void Awake()
    {
        CalculateStats();
    }

    /// <summary>
    /// Update is called every frame.
    /// </summary>
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    TakeDamage(10);
        //}
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        //Play audio clip
        Debug.Log("OOF");
    }
    /// <summary>
    /// Calculates the stats of the enemy. Can be ignored and set in the editor for special cases/mobs, otherwise standard formula will take precedence.
    /// </summary>
    public override void CalculateStats()
    {
        //10 is the base value for these three variables. That is to say that at level 1, these are your levels. If m_isLinked is set to true, enemies will follow this pattern. Untick m_isLinked for unique, or to set up enemies manually.

        if (m_isLinked)
        {
            m_movementSpeed.SetValue(10 + m_level);
            m_vitality.SetValue(10 + m_level);
            m_strength.SetValue(10 + m_level);

            m_maxHealth = m_vitality.GetValue() * (5);

            m_damage.SetValue(m_strength.GetValue() * (2));

            m_currentInsanity = 10;
        }
    }

    /// <summary>
    /// The  enemy death event. //TODO Add an animation/shader to show death, instead of destroying the gameobject.
    /// </summary>
    public override void Die()
    {
        base.Die();
        
        //TODO give experience/gold to the player who killed it
    }
}
