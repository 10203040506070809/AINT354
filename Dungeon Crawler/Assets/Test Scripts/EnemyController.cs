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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_lookRadius);
    }
}
