using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// A reference to the target, I.E the player.
    /// </summary>
     private GameObject m_target = null;
    /// <summary>
    /// The distance between the player and the camera.
    /// </summary>
    [SerializeField] private float distance = 0f;


    private void Start()
    {
        
    }

    private void Awake()
    {
        

    }
    /// <summary>
    /// Makes certain the distance between the camera and the player is always the same.
    /// </summary>
    void LateUpdate()
    {
        if (m_target == null)
        {
            m_target = GameObject.FindGameObjectWithTag("Player");
        }
        transform.position = new Vector3(m_target.transform.position.x, distance, m_target.transform.position.z);
        
    }
}

