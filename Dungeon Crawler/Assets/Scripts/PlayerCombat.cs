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
    [SerializeField] private CharacterStats m_myStats;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Animator m_playerAnimator;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ParticleSystem m_bloodSystem;
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

                    other.gameObject.GetComponent<EnemyStats>().TakeDamage((int)m_myStats.GetAttack());
                    other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(10, 10, 10));
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
