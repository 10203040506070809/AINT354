using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterMovement
{
    [SerializeField] private float m_lookRadius;
    [SerializeField] private GameObject m_target;
    private float m_lastAttacked;
    private NavMeshAgent m_navMeshAgent;
    

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
       
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        Move();
    }

    public override void Move()
    {
        float distance = Vector3.Distance(m_target.transform.position, transform.position);
        if (distance <= m_lookRadius)
        {
            m_navMeshAgent.SetDestination(m_target.transform.position);

            if(distance <= m_navMeshAgent.stoppingDistance)
            {
                ///Do attack
                Attack();
                ///Face target
                FaceTarget();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_lookRadius);
    }
    
    private void FaceTarget()
    {

    }
    private void Attack()
    {
        
        CharacterStats myStats = GetComponent<EnemyStats>();
        CharacterStats targetStats = m_target.GetComponent<PlayerStats>();
        int attackSpeed = (int)myStats.GetAttackSpeed();
        if (m_lastAttacked >= attackSpeed)
        {
            targetStats.TakeDamage((int)(myStats.GetAttack() - myStats.GetArmour()));
            m_lastAttacked = 0;
            //Debug.Log("Attacked - " + m_lastAttacked);
        }
        else
        {
            m_lastAttacked += Time.deltaTime;
            //Debug.Log("Did not attack - " + m_lastAttacked);
        }
    }
}
