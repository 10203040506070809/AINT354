using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    /// <summary>
    /// A vector denoting the direction of the raycast.
    /// </summary>
    private Vector3 m_direction;
    private GameObject map;
    private Vector3 tempVec;
    /// <summary>
    /// A float value denoting the distance the raycast will check.
    /// </summary>
    [SerializeField] private float m_maxDistance = 0;

    // Update is called once per frame
    void Update()
    {
        m_direction = transform.TransformDirection(Vector3.down);
        map = GameObject.FindGameObjectWithTag("Map");
        RaycastHit hit;

        ///If the raycast hits something within the max distance in the direction of m_direction
        if (Physics.Raycast(transform.position, m_direction, out hit, m_maxDistance))
        {
            Debug.Log("here");
            ///Check if the hit object has an interactable script
            if (hit.collider.gameObject.transform.parent.tag == "Tile")
            {
                tempVec = hit.collider.gameObject.transform.position;
                map.transform.GetChild(40-(Mathf.RoundToInt((tempVec.z)/40)*9)+(Mathf.RoundToInt(tempVec.x/40))).GetComponent<Image>().color = Color.green;
                Debug.Log(hit.collider.gameObject.transform.parent.name);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(0, m_maxDistance, 0));
    }
}
