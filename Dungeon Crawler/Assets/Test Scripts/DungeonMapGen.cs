using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public int m_fillPercent;
    /// <summary>
    /// Matrix holding the map data consisting of 1s and 0s. 1 means there is a wall, 0 means there is air.
    /// </summary>
    int[,] m_map;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        m_map = new int[m_width, m_height];
        RandomlyFillMap();
    }
    
    /// <summary>
    /// Generates a random seed if set by user then populates m_map with 1s and 0s.
    /// </summary>
    void RandomlyFillMap()
    {
        if (m_useRandomSeed)
        {
            m_seed = Time.time.ToString();
        }
        System.Random pseudoRandom = new System.Random(m_seed.GetHashCode());

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (pseudoRandom.Next(0,100) < m_fillPercent)
                {
                    m_map[i, j] = 1;
                }
                else
                {
                    m_map[i, j] = 0;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (m_map != null)
        {
            for (int i = 0; i < m_width; i++)
            {
                for (int j = 0; j < m_height; j++)
                {
                    if (m_map[i, j] == 1)
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }
                    Vector3 pos = new Vector3(-m_width / 2 + i + .5f, 0, -m_height / 2 + j + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
