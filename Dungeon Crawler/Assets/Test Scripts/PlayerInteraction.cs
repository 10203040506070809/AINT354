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
        m_direction = transform.TransformDirection(Vector3.forward);
    }
    /// <summary>
    /// Called at the same time once per frame. Checks in a direction for  an interactable object.
    /// </summary>
    private void FixedUpdate()
    {
        RaycastHit hit;

        
        if (Physics.Raycast(transform.position, m_direction, out hit, m_maxDistance))
        {
            if(hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    hit.collider.gameObject.GetComponent<Interactable>().InteractedWith();
                }
            }
            
        }
    }
}
