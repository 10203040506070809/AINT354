using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject m_target;
    Vector3 offset;

    void Start()
    {
        offset = m_target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = m_target.transform.position - (offset);
    }
}

