using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
    public GameObject[] mapTiles;
    public GameObject end;
    private List<List<GameObject>> connectionList = new List<List<GameObject>>();
    private List<GameObject> northConnection = new List<GameObject>();
    private List<GameObject> eastConnection = new List<GameObject>();
    private List<GameObject> southConnection = new List<GameObject>();
    private List<GameObject> westConnection = new List<GameObject>();
    private GameObject startTile;
    public GameObject justNorth;
    public GameObject justEast;
    public GameObject justSouth;
    public GameObject justWest;
    public int length;
    private tile temp;
    private GameObject tempTile;
    private Vector3 tempPos;
    int[,] map = new int[10, 10];
    private List<tile> tileList = new List<tile>();
    private List<tile> tempTileList = new List<tile>();
    private List<GameObject> TempPossibleTileList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StoreTiles();
        //Debug.Log(northConnection.Count);
        startTile = mapTiles[Random.Range(0, mapTiles.Length - 1)];
        tileList.Add(new tile(startTile, 5, 5, new Vector3(0, 0, 0), startTile.name));
        map[5, 5] = 1;
        Instantiate(tileList[Random.Range(0,tileList.Count - 1)].type, tileList[0].worldPosition, Quaternion.identity);
        //LoadSurroundingTiles(tileList[0]);
        bool changed = true;
        do
        {
            while (tileList.Count < length && changed)
            {
                changed = false;
                Debug.Log("here");
                foreach (tile itile in tileList)
                {
                    if (!itile.full)
                    {
                        Debug.Log(map);
                        LoadSurroundingTiles(itile);
                        changed = true;
                    }
                }
                foreach (tile itile in tempTileList)
                {
                    tileList.Add(itile);
                }
                tempTileList.Clear();
            }
        }
        while (tileList.Count < length);

        changed = true;
        while (changed)
        {
            changed = false;
            foreach (tile itile in tileList)
            {
                if (!itile.full)
                {
                    Debug.Log(map);
                    FinishTiles(itile);
                    changed = true;
                }
            }
        }
        Instantiate(end,tileList[tileList.Count - 1].worldPosition, Quaternion.identity);
    }
    private void LoadSurroundingTiles(tile currentTile)
    {
        if (currentTile.config[0] == '1' && currentTile.mapY != 0 && map[currentTile.mapX,currentTile.mapY - 1] != 1)
        {
            if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX, currentTile.mapY - 2] == 0 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
            {
                tempTile = connectionList[2][Random.Range(0, connectionList[2].Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - 500);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX, currentTile.mapY - 1, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }
            else
            {
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX, currentTile.mapY - 2] == 0 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX, currentTile.mapY - 2] == 1 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX, currentTile.mapY - 2] == 0 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 1)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX, currentTile.mapY - 2] == 1 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX, currentTile.mapY - 2] == 0 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 1)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX, currentTile.mapY - 2] == 1 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 1)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX, currentTile.mapY - 2] == 1 && map[currentTile.mapX + 1, currentTile.mapY - 1] == 1)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(TempPossibleTileList.Count);
                tempTile = TempPossibleTileList[Random.Range(0, TempPossibleTileList.Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - 500);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX, currentTile.mapY - 1, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                TempPossibleTileList.Clear();
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }


        }
        if (currentTile.config[1] == '1' && currentTile.mapX != 9 && map[currentTile.mapX + 1, currentTile.mapY] != 1)
        {
            if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX + 2, currentTile.mapY] == 0 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
            {
                tempTile = connectionList[3][Random.Range(0, connectionList[3].Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x - 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX + 1, currentTile.mapY, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }
            else
            {
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX + 2, currentTile.mapY] == 0 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX + 2, currentTile.mapY] == 1 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX + 2, currentTile.mapY] == 0 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX + 2, currentTile.mapY] == 1 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX + 2, currentTile.mapY] == 0 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX + 2, currentTile.mapY] == 1 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX + 2, currentTile.mapY] == 1 && map[currentTile.mapX + 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(TempPossibleTileList.Count);
                tempTile = TempPossibleTileList[Random.Range(0, TempPossibleTileList.Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x - 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX + 1, currentTile.mapY, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                TempPossibleTileList.Clear();
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }

        }
        if (currentTile.config[2] == '1' && currentTile.mapY != 9 && map[currentTile.mapX, currentTile.mapY + 1] != 1)
        {
            if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && map[currentTile.mapX, currentTile.mapY + 2] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
            {
                tempTile = connectionList[0][Random.Range(0, connectionList[0].Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + 500);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX, currentTile.mapY + 1, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }
            else
            {
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 1 && map[currentTile.mapX, currentTile.mapY + 2] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && map[currentTile.mapX, currentTile.mapY + 2] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && map[currentTile.mapX, currentTile.mapY + 2] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 1 && map[currentTile.mapX, currentTile.mapY + 2] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 1 && map[currentTile.mapX, currentTile.mapY + 2] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && map[currentTile.mapX, currentTile.mapY + 2] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[2] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX + 1, currentTile.mapY + 1] == 1 && map[currentTile.mapX, currentTile.mapY + 2] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(TempPossibleTileList.Count);
                tempTile = TempPossibleTileList[Random.Range(0, TempPossibleTileList.Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + 500);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX, currentTile.mapY + 1, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                TempPossibleTileList.Clear();
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }
        }
        if (currentTile.config[3] == '1' && currentTile.mapX != 0 && map[currentTile.mapX - 1, currentTile.mapY] != 1)
        {
            if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX - 2, currentTile.mapY] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
            {
                tempTile = connectionList[1][Random.Range(0, connectionList[1].Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x + 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX - 1, currentTile.mapY, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }
            else
            {
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX - 2, currentTile.mapY] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX - 2, currentTile.mapY] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX - 2, currentTile.mapY] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX - 2, currentTile.mapY] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX - 2, currentTile.mapY] == 0 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && map[currentTile.mapX - 2, currentTile.mapY] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[3] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (map[currentTile.mapX - 1, currentTile.mapY - 1] == 1 && map[currentTile.mapX - 2, currentTile.mapY] == 1 && map[currentTile.mapX - 1, currentTile.mapY + 1] == 1)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                Debug.Log(TempPossibleTileList.Count);
                tempTile = TempPossibleTileList[Random.Range(0, TempPossibleTileList.Count - 1)];
                tempPos = new Vector3(currentTile.worldPosition.x + 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
                Instantiate(tempTile, tempPos, Quaternion.identity);
                temp = new tile(tempTile, currentTile.mapX - 1, currentTile.mapY, tempPos, tempTile.name);
                map[temp.mapX, temp.mapY] = 1;
                tempTileList.Add(temp);
                TempPossibleTileList.Clear();
                //currentTile.neighbour.Add(new tile(tempTile, tempPos, tempTile.name));
            }
        }
        currentTile.full = true;
    }
    // Update is called once per frame
    private void FinishTiles(tile currentTile)
    {
        if (currentTile.config[0] == '1')
        {
            tempTile = justSouth;
            tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - 500);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX, currentTile.mapY - 1, tempPos, tempTile.name);
        }
        if (currentTile.config[1] == '1')
        {
            tempTile = justWest;
            tempPos = new Vector3(currentTile.worldPosition.x - 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX + 1, currentTile.mapY, tempPos, tempTile.name);
        }
        if (currentTile.config[2] == '1')
        {
            tempTile = justNorth;
            tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + 500);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX, currentTile.mapY + 1, tempPos, tempTile.name);
        }
        if (currentTile.config[3] == '1')
        {
            tempTile = justEast;
            tempPos = new Vector3(currentTile.worldPosition.x + 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX - 1, currentTile.mapY, tempPos, tempTile.name);
        }
        currentTile.full = true;
    }
    void Update()
    {
        
    }
    private class tile
    {
        public GameObject type;
        public Vector3 worldPosition;
        public string config;
        public int mapX;
        public int mapY;
        //public List<tile> neighbour = new List<tile>();
        public bool full;
        public tile()
        {

        }
        public tile(GameObject typ, int X, int Y, Vector3 pos, string con)
        {
            type = typ;
            mapX = X;
            mapY = Y;
            worldPosition = pos;
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
