using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GameObject m_weapon;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private CharacterStats m_myStats = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Animator m_playerAnimator = null;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ParticleSystem m_bloodSystem = null;
    private CharacterStats m_enemyStats;
    /// <summary>
    /// 
    /// </summary>
     private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_playerAnimator.GetBool("isAttacking") == true)
        {
            if (other.tag == "Enemy")
            {
                if (other.gameObject.GetComponent<EnemyStats>() != null)
                {
                    m_enemyStats = other.gameObject.GetComponent<EnemyStats>();

                    m_enemyStats.TakeDamage((int)m_myStats.GetAttack());
                    
                  if (m_enemyStats.m_currentHealth <= 0)
                    {
                        ///Gives gold and experience on kill
                        m_myStats.m_gold.SetValue(m_myStats.m_gold.GetValue() + m_enemyStats.m_gold.GetValue());
                        m_myStats.m_experience.SetValue(m_myStats.m_experience.GetValue() + m_enemyStats.m_experience.GetValue());
                        PlayerPrefs.SetInt("Gold", m_myStats.m_gold.GetValue());
                        Debug.Log("Got gold and experience!");
                    }
                    ParticleSystem Clone = Instantiate(m_bloodSystem, other.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                    Destroy(Clone.gameObject, 1.0f);
                }
                else
                {
                    Debug.Log("Enemy does not have an enemystats script");
                }
            }
        }
    }

}
