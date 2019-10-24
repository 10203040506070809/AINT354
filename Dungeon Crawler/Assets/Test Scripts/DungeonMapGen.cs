using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGen : MonoBehaviour
{
    public int m_width;
    public int m_height;

    public string m_seed;
    public bool m_useRandomSeed;

    [Range(0, 100)]
    public int m_randomFillPercent;

    int[,] m_map;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

}
