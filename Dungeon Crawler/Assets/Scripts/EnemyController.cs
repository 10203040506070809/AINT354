using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterMovement
{
    /// <summary>
    /// A variable to store the look radius of a character. Within this radius, the character will search for the target.
    /// </summary>
    [SerializeField] private float m_lookRadius = 0;
    /// <summary>
    /// The target the current character is attempting to follow.
    /// </summary>
    [SerializeField] private GameObject m_target = null;
    /// <summary>
    /// A float value to denote when the character last attacked/
    /// </summary>
    private float m_lastAttacked;
    /// <summary>
    /// A reference to the NavMeshAgent GameObject.
    /// </summary>
    private NavMeshAgent m_navMeshAgent;
    /// <summary>
    /// attackSpeed is an integer derived from the enemyStats script.
    /// </summary>
    private int m_attackSpeed = 0;
    /// <summary>
    /// A reference to the current GameObjects EnemyStats script, used to get variable values.
    /// </summary>
    CharacterStats m_myStats;
    /// <summary>
    /// In this method variables are initialised.
    /// </summary>
    void Start()
    {
       
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_myStats = GetComponent<EnemyStats>();
        
        m_attackSpeed = (int)m_myStats.GetAttackSpeed();
        m_target = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Update occurs during every frame, so input is taken using the Move method every frame.
    /// </summary>
    public void Update()
    {
        Move();
    }
    /// <summary>
    /// Moves the character if the distance from the target is less than the look radius. Uses navmesh to avoid obstacles.
    /// </summary>
    public override void Move()
    {
        ///A variable denoting the distance between the current character and its target.
        float distance = Vector3.Distance(m_target.transform.position, transform.position);
        if (distance <= m_lookRadius)
        {
            m_navMeshAgent.SetDestination(m_target.transform.position);
            if (distance <= m_navMeshAgent.stoppingDistance)
            {
                ///Do attack
                Attack();

            }
            ///This is reset when the player leaves stopping distance so the enemy instantly attacks whenever the player moves. This may not be required when actual combat is implemented due to knockback, but is a reasonable failsafe.
            else
            {
                m_lastAttacked = m_attackSpeed;
            }
        }
    }
    /// <summary>
    /// Draws a wireframe sphere  around the character within the editor to easily see its look radius.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_lookRadius);
    }

    /// <summary>
    /// Attacks the target when within the stopping distance of the character. 
    /// </summary>
    private void Attack()
    {

        /// A reference to the target GameObjects PlayerStats script, used to get variable values.
        CharacterStats targetStats = m_target.GetComponent<PlayerStats>();

        if (m_lastAttacked >= m_attackSpeed)
        {
            ///Clamps the value of the armour between 0 and the value of the targets armour
            targetStats.TakeDamage((int)(m_myStats.GetDamage() + targetStats.m_currentInsanity - (Mathf.Clamp(m_myStats.GetArmour(), 0, m_myStats.GetArmour()))));
            m_lastAttacked = 0;
            //Do attack animation
            
            //Debug.Log("Attacked - " + m_lastAttacked);
        }
        else
        {
            m_lastAttacked += Time.deltaTime;
            //Debug.Log("Did not attack - " + m_lastAttacked);
        }
    }
}
