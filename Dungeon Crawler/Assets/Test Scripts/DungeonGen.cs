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
    private tile temp;
    private GameObject tempTile;
    private Vector3 tempPos;
    private List<tile> tileList = new List<tile>();
    private List<tile> tempTileList = new List<tile>();
    // Start is called before the first frame update
    void Start()
    {
        StoreTiles();
        startTile = mapTiles[Random.Range(0, mapTiles.Length - 1)];
        tileList.Add(new tile(startTile, new Vector3(0, 0, 0), startTile.name));
        Instantiate(tileList[0].type, tileList[0].position, Quaternion.identity);
        LoadSurroundingTiles(tileList[0]);
        while (tileList.Count < length)
        {
            Debug.Log("here");
            foreach (tile itile in tileList)
            {
                if (!itile.full)
                {
                    Debug.Log("here");
                    LoadSurroundingTiles(itile);
                }
            }
            foreach (tile itile in tempTileList)
            {
                tileList.Add(itile);
            }
            tempTileList.Clear();
        }

    }
    private void LoadSurroundingTiles(tile currentTile)
    {
        if (currentTile.config[0] == '1')
        {
            tempTile = connectionList[2][Random.Range(0, connectionList[2].Count - 1)];
            tempPos = new Vector3(currentTile.position.x, currentTile.position.y, currentTile.position.z - 500);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, tempPos, tempTile.name);
            tempTileList.Add(temp);
            //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
        }
        if (currentTile.config[1] == '1')
        {
            tempTile = connectionList[3][Random.Range(0, connectionList[3].Count - 1)];
            tempPos = new Vector3(currentTile.position.x - 500, currentTile.position.y, currentTile.position.z);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, tempPos, tempTile.name);
            tempTileList.Add(temp);
            //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
        }
        if (currentTile.config[2] == '1')
        {
            tempTile = connectionList[0][Random.Range(0, connectionList[0].Count - 1)];
            tempPos = new Vector3(currentTile.position.x, currentTile.position.y, currentTile.position.z + 500);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, tempPos, tempTile.name);
            tempTileList.Add(temp);
            //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
        }
        if (currentTile.config[3] == '1')
        {
            tempTile = connectionList[1][Random.Range(0, connectionList[1].Count - 1)];
            tempPos = new Vector3(currentTile.position.x + 500, currentTile.position.y, currentTile.position.z);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, tempPos, tempTile.name);
            tempTileList.Add(temp);
            //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
        }
        currentTile.full = true;
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
        //public List<tile> neighbour = new List<tile>();
        public bool full;
        public tile()
        {

        }
        public tile(GameObject typ, Vector3 pos, string con)
        {
            type = typ;
            position = pos;
            config = con;
            full = false;
        }
    }
    private void StoreTiles()
    {
        foreach (GameObject section in mapTiles)
        {
            if (section.name[0] == '1')
            {
                northConnection.Add(section);
            }
            if (section.name[1] == '1')
            {
                eastConnection.Add(section);
            }
            if (section.name[2] == '1')
            {
                southConnection.Add(section);
            }
            if (section.name[3] == '1')
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
