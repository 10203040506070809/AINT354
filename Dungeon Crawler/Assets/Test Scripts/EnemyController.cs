using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterMovement
{
    [SerializeField] private float m_lookRadius;
    [SerializeField] private GameObject m_target;
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
        Vector3 direction = (m_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void Attack()
    {
        CharacterStats enemyStats = m_target.GetComponent<CharacterStats>();
        CharacterStats myStats = this.GetComponent<CharacterStats>();
    }
}
