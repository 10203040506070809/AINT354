using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{

    
    
    // Start is called before the first frame update
    private void Start()
    {
        CalculateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    /// <summary>
    /// Calculates the stats of the enemy. Can be ignored and set in the editor for special cases/mobs, otherwise standard formula will take precedence.
    /// </summary>
    public override void CalculateStats()
    {
        //10 is the base value for these three variables. That is to say that at level 1, these are your levels.
        m_movementSpeed.SetValue(10 + m_level);
        m_vitality.SetValue(10 + m_level);
        m_strength.SetValue(10 + m_level);

        m_maxHealth = m_vitality.GetValue() * (5);
        
        m_damage.SetValue(m_strength.GetValue() * (2));
    }

    /// <summary>
    /// The  enemy death event. //TODO Add an animation/shader to show death, instead of destroying the gameobject.
    /// </summary>
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
