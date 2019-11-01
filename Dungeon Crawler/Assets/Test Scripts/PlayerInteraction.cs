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
    [SerializeField] private float m_maxDistance = 0;

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
        m_direction = transform.TransformDirection(Vector3.forward);
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
    /// <summary>
    /// Draws a wireframe line from the character to easily see its 
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,new Vector3(0,0,m_maxDistance));
    }
}
