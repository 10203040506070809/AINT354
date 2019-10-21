
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private void Start()
    {
        CalculateStats();
    }

    /// <summary>
    /// An update which is called every frame.
    /// </summary>
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    TakeDamage(10);
        //}
    }
    /// <summary>
    /// A method which calculates stats according to a formula. This allows variation between players and specific enemies.
    /// </summary>
    public override void CalculateStats()
    {
        //10 is the base value for these three variables. That is to say that at level 1, these are your levels.
        m_movementSpeed.SetValue(10 + m_level);
        m_vitality.SetValue(10 + m_level);
        m_strength.SetValue(10 + m_level);
        


        m_maxHealth = m_vitality.GetValue() * 10;

        m_damage.SetValue(m_strength.GetValue() * 5);

    }

    /// <summary>
    /// The  player death event. //TODO Add an animation/shader to show death, instead of destroying the gameobject.
    /// </summary>
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
        
        //TODO on death, give gold/experience to the thing that killed it.
    }
}
