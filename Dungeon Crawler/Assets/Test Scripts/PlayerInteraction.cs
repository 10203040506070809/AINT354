using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private Vector3 m_direction;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private float m_maxDistance;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        
    }
    /// <summary>
    /// Called at the same time once per frame. Checks in a direction for  an interactable object.
    /// </summary>
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, m_direction, m_maxDistance))
        {

        }
    }
}
