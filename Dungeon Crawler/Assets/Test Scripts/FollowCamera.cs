using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// A reference to the target, I.E the player.
    /// </summary>
    [SerializeField] private GameObject m_target = null;
    /// <summary>
    /// The distance between the player and the camera.
    /// </summary>
    Vector3 offset;

    /// <summary>
    /// This method is called when the application is launched. Gets the distance between the player and camera.
    /// </summary>
    void Start()
    {
        offset = m_target.transform.position - transform.position;
    }

    /// <summary>
    /// Makes certain the distance between the camera and the player is always the same.
    /// </summary>
    void LateUpdate()
    {
        transform.position = m_target.transform.position - (offset);
    }
}

