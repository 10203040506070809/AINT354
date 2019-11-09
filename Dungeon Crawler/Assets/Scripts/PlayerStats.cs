
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

        ///If in the unity editor, use stats from inspector - This is useful for debugging.
#if UNITY_EDITOR
        Debug.Log("Using Unity Editor");
#endif

        ///In standalone, prefer using the player level from playerprefs.
#if UNITY_STANDALONE
        m_level = PlayerPrefs.GetInt("Level", 1);
        m_gold.SetValue(PlayerPrefs.GetInt("Gold", 1));
        m_experience.SetValue(PlayerPrefs.GetInt("Experience", 0));
#endif
        CalculateStats();
    }



    /// <summary>
    /// An update which is called every frame.
    /// </summary>
    private void Update()
    {
        CheckForLevelUp();
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

            m_damage.SetValue(m_strength.GetValue() * 2);
        }
    }

    /// <summary>
    /// The  player death event. //TODO Add an animation/shader to show death, instead of destroying the gameobject.
    /// </summary>
    public override void Die()
    {
        //this.gameObject.SetActive(false);

        this.gameObject.transform.position = new Vector3(0, 0, 0);
        m_currentHealth = m_maxHealth;
        m_currentInsanity = 0;
        //TODO on death, give gold/experience to the thing that killed it.
    }
    /// <summary>
    /// After the player is hit, reset animation
    /// </summary>
    private void HitReset()
    {
        m_animator.SetBool("isHit", false);
    }
    /// <summary>
    /// Checks for a level up to increase player level - Put experience formula in here.
    /// </summary>
    private void CheckForLevelUp()
    {

    }

}
