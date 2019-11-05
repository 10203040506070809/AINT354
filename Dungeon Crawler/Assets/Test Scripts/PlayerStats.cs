
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Animator m_animator;



    /// <summary>
    /// A method that is called when the script is loaded.
    /// </summary>
    private void Awake()
    {
        m_animator = this.GetComponent<Animator>();
        CalculateStats();
    }



    /// <summary>
    /// An update which is called every frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        //Play audio clip
        Debug.Log("OOF");
        m_animator.SetBool("isHit", true);
        Invoke("HitReset", 0.5f);
        
    }
    /// <summary>
    /// A method which calculates stats according to a formula. This allows variation between players and specific enemies.
    /// </summary>
    public override void CalculateStats()
    {
        //10 is the base value for these three variables. That is to say that at level 1, these are your levels. if m_isLinked is set to true, enemies will follow this pattern. Untick m_isLinked for unique, or to set up enemies manually.

        if (m_isLinked)
        {
            m_movementSpeed.SetValue(10 + m_level);
            m_vitality.SetValue(10 + m_level);
            m_strength.SetValue(10 + m_level);



            m_maxHealth = m_vitality.GetValue() * 10;

            m_damage.SetValue(m_strength.GetValue() * 5);
        }
    }

    /// <summary>
    /// The  player death event. //TODO Add an animation/shader to show death, instead of destroying the gameobject.
    /// </summary>
    public override void Die()
    {
        this.gameObject.SetActive(false);
        
        
        //TODO on death, give gold/experience to the thing that killed it.
    }

    private void HitReset()
    {
        m_animator.SetBool("isHit", false);
    } 
}
