using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    int m_health = 2;
    public void TakeDamage()
    {
        m_health -= 1;
        if (m_health <= 0)
        {
            gameObject.GetComponent<DropItem>().Drop(gameObject.transform.position, gameObject);
            Destroy(gameObject);
        }
    }
}
