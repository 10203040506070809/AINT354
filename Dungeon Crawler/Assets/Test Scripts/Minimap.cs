using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    /// <summary>
    /// A vector denoting the direction of the raycast.
    /// </summary>
    private Vector3 m_direction;
    /// <summary>
    /// The map UI panel.
    /// </summary>
    private GameObject map;
    /// <summary>
    /// The map UI overlay which tracks player position.
    /// </summary>
    private GameObject playerPositionMap;
    /// <summary>
    /// The position of the tile the player is in.
    /// </summary>
    private Vector3 tempVec;
    /// <summary>
    /// The tile the player is in.
    /// </summary>
    private GameObject tempTile;
    /// <summary>
    /// An array the size of the map UI which tracks whether the player has already visited a room.
    /// </summary>
    private int[,] grid = new int[9,9];
    /// <summary>
    /// Array of UI tile assets.
    /// </summary>
    public Sprite[] imgs;
    /// <summary>
    /// Sprite which represents the players position.
    /// </summary>
    public Sprite marker;
    /// <summary>
    /// Sprite used to clear the player position once they leave a room.
    /// </summary>
    public Sprite clear;
    /// <summary>
    /// A float value denoting the distance the raycast will check.
    /// </summary>
    [SerializeField] private float m_maxDistance = 0;

    // Update is called once per frame
    void Update()
    {
        UpdateMap();
    }
    private void UpdateMap()
    {
        /// Used to set the raycast to be facing downwards.
        m_direction = transform.TransformDirection(Vector3.down);
        /// Used to get the UI component for the map
        map = GameObject.FindGameObjectWithTag("MapTiles");
        /// Used to get the UI component for the player position.
        playerPositionMap = GameObject.FindGameObjectWithTag("MapPlayer");
        RaycastHit hit;

        ///If the raycast hits something within the max distance in the direction of m_direction
        if (Physics.Raycast(transform.position, m_direction, out hit, m_maxDistance))
        {
            ///Check if the hit object has an interactable script
            if (hit.collider.gameObject.transform.parent.tag == "Tile")
            {
                /// Gets the tile the player is standing on.
                tempTile = hit.collider.gameObject.transform.parent.gameObject;
                /// Gets the world position of the tile the player is standing on.
                tempVec = tempTile.transform.position;
                /// Gets the binary representation of the tile.
                string str = tempTile.name.Substring(0, 4);
                /// Clears the UI representing the player position.
                for (int i = 0; i < 81; i++)
                {
                    playerPositionMap.transform.GetChild(i).GetComponent<Image>().sprite = clear;
                }
                /// Places the player marker on the UI which represents the room the player is in.
                playerPositionMap.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9)) + (Mathf.RoundToInt(tempVec.x / 40))).GetComponent<Image>().sprite = marker;
                /// Updates the UI which represents the map with the asset which represents the tile the player is on.
                map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9)) + (Mathf.RoundToInt(tempVec.x / 40))).GetComponent<Image>().sprite = imgs[(Convert.ToInt32(str, 2)) - 1];
                /// Updates the 2D array to say that the player has entered the room and the graphic no longer needs to be changed.
                grid[Mathf.RoundToInt(tempVec.x / 40) + 4, 4 - Mathf.RoundToInt(tempVec.z / 40)] = 1;
                /// Checks if the room has a north connection.
                if (tempTile.name[0] == '1')
                {
                    /// Checks if the UI square to the north has already been populated.
                    if (grid[Mathf.RoundToInt(tempVec.x / 40) + 4, 3 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the north.
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) - 9).GetComponent<Image>().sprite = imgs[16];
                    }
                }
                /// Checks if the room has a east connection.
                if (tempTile.name[1] == '1')
                {
                    /// Checks if the UI square to the east has already been populated.
                    if (grid[Mathf.RoundToInt(tempVec.x / 40) + 5, 4 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the east.
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) + 1).GetComponent<Image>().sprite = imgs[15];
                    }
                }
                /// Checks if the room has a south connection.
                if (tempTile.name[2] == '1')
                {
                    /// Checks if the UI square to the south has already been populated.
                    if (grid[Mathf.RoundToInt(tempVec.x / 40) + 4, 5 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the south.
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) + 9).GetComponent<Image>().sprite = imgs[17];
                    }
                }
                /// Checks if the room has a west connection.
                if (tempTile.name[3] == '1')
                {
                    /// Checks if the UI square to the west has already been populated.
                    if (grid[3 + Mathf.RoundToInt(tempVec.x / 40), 4 - Mathf.RoundToInt(tempVec.z / 40)] != 1)
                    {
                        /// Updates the UI with an unknown tile to the west.
                        map.transform.GetChild((40 - (Mathf.RoundToInt(tempVec.z / 40) * 9) + (Mathf.RoundToInt(tempVec.x / 40))) - 1).GetComponent<Image>().sprite = imgs[18];
                    }
                }
            }
        }
    }
}
