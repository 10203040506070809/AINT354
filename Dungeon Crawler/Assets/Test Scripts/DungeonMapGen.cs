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
        ///Initialises m_map with chosen parameters
        m_map = new int[m_width, m_height];
        ///Basic instantiation of map.
        RandomlyFillMap();
        ///Performs the smoothing algorithm a set number of times.
        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
        DungeonMeshGen meshGen = GetComponent<DungeonMeshGen>();
        meshGen.GenerateMesh(m_map, 1);
    }
    
    /// <summary>
    /// Generates a random seed if set by user then populates m_map with 1s and 0s. Edges of the map are set to walls.
    /// </summary>
    void RandomlyFillMap()
    {
        ///Checks if a random seed needs to be created. If it does then it creates on based on current time.
        if (m_useRandomSeed)
        {
            m_seed = Time.time.ToString();
        }
        System.Random pseudoRandom = new System.Random(m_seed.GetHashCode());
        ///Cycles through the map matrix.
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                ///Sets the perimeter values of the m_map matrix to be 1 (walls).
                if (i == 0 || i == m_width - 1 || j == 0 || j == m_height - 1)
                {
                    m_map[i, j] = 1;
                }
                ///Randomly fills the rest of the m_map matrix.
                else
                {
                    if (pseudoRandom.Next(0, 100) < m_fillPercent)
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
    }
    /// <summary>
    /// Checks the surrounding values of every point in the m_map matrix. If the point is surrounded by more than x number of walls, the point is set to a wall. If the point is surrounded by less than x number of walls, the point is set to air.
    /// </summary>
    void SmoothMap()
    {
        ///Loops through m_map matrix.
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                ///Passes the current point into 'GetSurroundingWallCount' which returns how many wall sections are surrounding the current point.
                int neighbourWallTiles = GetSurroundingWallCount(i, j);
                ///Changes the current point to a 1 (wall) if it is surrounded by more than x walls.
                if (neighbourWallTiles > 4)
                    m_map[i, j] = 1;
                ///Changes the current point to a 1 (air) if it is surrounded by less than x walls.
                else if (neighbourWallTiles < 4)
                    m_map[i, j] = 0;

            }
        }
    }

    /// <summary>
    /// Counts how many walls are surrounding a given point.
    /// </summary>
    /// <param name="gridX">The value of the x coordinate being checked.</param>
    /// <param name="gridY">The value of the y coordinate being checked.</param>
    /// <returns>How many walls are surrounding the point (param1, param2).</returns>

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        ///Initialises wallCount which tracks how many walls are surrounding a given point.
        int wallCount = 0;
        ///Loops through every point in a 3x3 grid centered around the point passed into this method.
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                ///Check to avoid edge points of the map.
                if (neighbourX >= 0 && neighbourX < m_width && neighbourY >= 0 && neighbourY < m_height)
                {
                    ///Check to avoid the center point of the 3x3 grid being checked.
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        ///If the neighbour point is a wall, this is represented as a 1 in the m_map matrix so wallCount increases by 1. Air tiles are represented as 0 so do not contribute to wallCount.
                        wallCount += m_map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        ///How many walls are surrounding the point (param1, param2)
        return wallCount;
    }

    /// <summary>
    /// Draws the matrix m_map in unity using black and white squares. Black squares are walls, white squares are air.
    /// </summary>
    void OnDrawGizmos()
    {
        ///Checks if the map is initialised.
        if (m_map != null)
        {
            ///Loop through every point in m_map.
            for (int i = 0; i < m_width; i++)
            {
                for (int j = 0; j < m_height; j++)
                {
                    ///Sets wall values to black.
                    if (m_map[i, j] == 1)
                    {
                        Gizmos.color = Color.black;
                    }
                    ///sets air values to white.
                    else
                    {
                        Gizmos.color = Color.white;
                    }
                    ///Defines the coordinates at which the m_map matrix value will be drawn.
                    Vector3 pos = new Vector3(-m_width / 2 + i + .5f, 0, -m_height / 2 + j + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
