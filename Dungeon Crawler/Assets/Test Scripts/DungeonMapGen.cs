using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGen : MonoBehaviour
{
    /// <summary>
    /// Map width.
    /// </summary>
    public int m_width;
    /// <summary>
    /// Map height.
    /// </summary>
    public int m_height;
    /// <summary>
    /// random number used to generate map.
    /// </summary>
    public string m_seed;
    /// <summary>
    /// Whether the user wants to input their own seed or have one randomly generated.
    /// </summary>
    public bool m_useRandomSeed;
    /// <summary>
    /// Percentage of wall tiles generated in the map.
    /// </summary>
    [Range(0, 100)]
    public int m_randomFillPercent;
    /// <summary>
    /// Matrix holding the map data consisting of 1s and 0s. 1 means there is a wall, 0 means there is air.
    /// </summary>
    int[,] m_map;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

}
