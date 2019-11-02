using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
    public GameObject[] mapTiles;
    private List<List<GameObject>> connectionList = new List<List<GameObject>>();
    private List<GameObject> northConnection = new List<GameObject>();
    private List<GameObject> eastConnection = new List<GameObject>();
    private List<GameObject> southConnection = new List<GameObject>();
    private List<GameObject> westConnection = new List<GameObject>();
    private GameObject startTile;
    public int length;
    private List<tile> tileList = new List<tile>();
    // Start is called before the first frame update
    void Start()
    {
        StoreTiles();
        startTile = mapTiles[Random.Range(0, mapTiles.Length - 1)];
        tileList.Add(new tile(startTile, new Vector3(0, 0, 0), startTile.name));
        Instantiate(tileList[0].type, tileList[0].position, Quaternion.identity);
        /*for (int i = 0; i < length; i++)
        {
            if (mapTiles[1].name[0] == 1)
            {
                Instantiate(connectionList[2][Random.Range(0, connectionList[2].Count - 1)], new Vector3(0, 0, 0), Quaternion.identity);
            }
            if (mapTiles[1].name[1] == 1)
            {
                Instantiate(connectionList[3][Random.Range(0, connectionList[3].Count - 1)], new Vector3(0, 0, 0), Quaternion.identity);
            }
            if (mapTiles[1].name[2] == 1)
            {
                Instantiate(connectionList[0][Random.Range(0, connectionList[0].Count - 1)], new Vector3(0, 0, 0), Quaternion.identity);
            }
            if (mapTiles[1].name[3] == 1)
            {
                Instantiate(connectionList[1][Random.Range(0, connectionList[1].Count - 1)], new Vector3(0, 0, 0), Quaternion.identity);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private class tile
    {
        public GameObject type;
        public Vector3 position;
        public string config;
        public int neighbour;
        public tile()
        {

        }
        public tile(GameObject typ, Vector3 pos, string con)
        {
            type = typ;
            position = pos;
            config = con;
        }
    }
    private void StoreTiles()
    {
        foreach (GameObject section in mapTiles)
        {
            if (section.name[0] == 1)
            {
                northConnection.Add(section);
            }
            if (section.name[1] == 1)
            {
                eastConnection.Add(section);
            }
            if (section.name[2] == 1)
            {
                southConnection.Add(section);
            }
            if (section.name[3] == 1)
            {
                westConnection.Add(section);
            }
            connectionList.Add(northConnection);
            connectionList.Add(eastConnection);
            connectionList.Add(southConnection);
            connectionList.Add(westConnection);
        }
    }
}
