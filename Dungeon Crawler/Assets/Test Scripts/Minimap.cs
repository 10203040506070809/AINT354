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
    private GameObject tempTile;
    private int[,] grid = new int[9,9];
    public Sprite img;
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
                tempTile = hit.collider.gameObject.transform.parent.gameObject;
                tempVec = tempTile.transform.position;
                map.transform.GetChild((40-(Mathf.RoundToInt(tempVec.z/40)*9))+(Mathf.RoundToInt(tempVec.x/40))).GetComponent<Image>().sprite = img;
                map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9)) + (Mathf.RoundToInt(tempVec.x / 40))).GetComponent<Image>().color = Color.gray;
                grid[Mathf.RoundToInt(tempVec.x / 40) + 4, 4 - Mathf.RoundToInt(tempVec.z / 40)] = 1;
                if (tempTile.name[0] == '1')
                {
                    if (grid[Mathf.RoundToInt(tempVec.x / 40) + 4, 3 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40)))-9).GetComponent<Image>().color = Color.blue;
                    }
                }
                if (tempTile.name[1] == '1')
                {
                    if (grid[Mathf.RoundToInt(tempVec.x / 40) + 5, 4 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) +1).GetComponent<Image>().color = Color.blue;
                    }
                }
                if (tempTile.name[2] == '1')
                {
                    if (grid[Mathf.RoundToInt(tempVec.x / 40) + 4, 5 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) + 9).GetComponent<Image>().color = Color.blue;
                    }
                }
                if (tempTile.name[3] == '1')
                {
                    if (grid[3 + Mathf.RoundToInt(tempVec.x / 40),4 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) -1).GetComponent<Image>().color = Color.blue;
                    }
                }
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
