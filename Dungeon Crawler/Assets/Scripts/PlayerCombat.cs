﻿using UnityEngine;

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
    private ItemBreak m_itemBreak;

    public bool m_isAttacking = false;
    /// <summary>
    /// 
    /// </summary>
     private void Update()
    {
        Attack();
    }

    /// <summary>
    /// If the object connects with a gameobject, activates this
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
       
        ///Checks if the player is currently attacking, so the player cant just walk into an enemy
        if (m_playerAnimator.GetBool("isAttacking") == true)
        {
            ///Checks if the gameobject is in fact an enemy
            if (other.tag == "Enemy")
            {
                ///Checks if the enemy has an enemystats script - Redundancy
                if (other.gameObject.GetComponent<EnemyStats>() != null)
                {
                    m_enemyStats = other.gameObject.GetComponent<EnemyStats>();

                    // m_enemyStats.TakeDamage((int)m_myStats.GetDamage() + (int)m_myStats.m_currentInsanity);
                    other.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward) * 500);
                    ///Checks if the enemy just died
                  if (m_enemyStats.m_currentHealth <= 0)
                    {
                        ///Gives gold and experience on kill
                        m_myStats.m_gold.SetValue(m_myStats.m_gold.GetValue() + m_enemyStats.m_gold.GetValue());
                        m_myStats.m_experience.SetValue(m_myStats.m_experience.GetValue() + m_enemyStats.m_experience.GetValue());
                        PlayerPrefs.SetInt("Gold", m_myStats.m_gold.GetValue());
                        Debug.Log("Got gold and experience!");
                    }
                  ///Creates a blood particle  effect
                    ParticleSystem Clone = Instantiate(m_bloodSystem, other.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                    Destroy(Clone.gameObject, 1.0f);
                }
                else
                {
                    Debug.LogWarning("Enemy does not have an enemystats script");
                }
            }
            /// Checks if the gameobject is a breakable object
            if (other.tag == "Breakable")
            {
                ///Checks if the object has an ItemBreak script - Redundancy
                if (other.gameObject.GetComponent<ItemBreak>() != null)
                {
                    m_itemBreak = other.gameObject.GetComponent<ItemBreak>();

                    m_itemBreak.TakeDamage();
                }
                else
                {
                    Debug.LogWarning("Object does not have an ItemBreak script");
                }
            }
        }
    }

    /// <summary>
    /// Allows the player to attack by pressing the Left Mouse Button
    /// </summary>
    private void Attack()
    {
        //Main attack - Slash
        if (Input.GetButton("Fire1"))
        {
           // if (m_playerAnimator.GetBool("isAttacking") == false)
            {
                m_playerAnimator.SetBool("isAttacking", true);
                Invoke("AttackCooldown", 1f);
            }
        }

        //Alt attack - Stab
        if (Input.GetButton("Fire2"))
        {
          //  if (m_playerAnimator.GetBool("isAttacking") == false)
            {
                m_playerAnimator.SetBool("isAttacking", true);
                Invoke("AttackCooldown", 0.5f);
            }
        }
    }
    /// <summary>
    /// After a set delay, re-enable attack
    /// </summary>
    private void AttackCooldown()
    {
        m_playerAnimator.SetBool("isAttacking", false);
    }

}
