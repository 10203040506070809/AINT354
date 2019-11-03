using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
    /// <summary>
    /// An array of game tiles consisting of every possible tile orientation.
    /// </summary>
    public GameObject[] mapTiles;
    /// <summary>
    /// The object which will trigger the end of the level.
    /// </summary>
    public GameObject end;
    /// <summary>
    /// List of tiles with a connection in the specified direction.
    /// </summary>
    private List<List<GameObject>> connectionList = new List<List<GameObject>>();
    private List<GameObject> northConnection = new List<GameObject>();
    private List<GameObject> eastConnection = new List<GameObject>();
    private List<GameObject> southConnection = new List<GameObject>();
    private List<GameObject> westConnection = new List<GameObject>();
    /// <summary>
    /// Where the player spawns.
    /// </summary>
    private tile startTile;
    private GameObject start;
    /// <summary>
    /// Tiles which are only accessible via the specified direction.
    /// </summary>
    public GameObject justNorth;
    public GameObject justEast;
    public GameObject justSouth;
    public GameObject justWest;
    /// <summary>
    /// The minimum size of the dungeon.
    /// </summary>
    public int length;
    /// <summary>
    /// The temporary tile which will be added to the list of all tiles.
    /// </summary>
    private tile temp;
    /// <summary>
    /// The temporary game object version of the tile to be added.
    /// </summary>
    private GameObject tempTile;
    /// <summary>
    /// The temporary position of the tile to be added.
    /// </summary>
    private Vector3 tempPos;
    /// <summary>
    /// Specifies the maximum dimensions of the map.
    /// </summary>
    private int[,] map;
    /// <summary>
    /// The list of all tiles as they are added to the map.
    /// </summary>
    private tile[,] claimed;
    private List<tile> tileList = new List<tile>();
    /// <summary>
    /// The list of tiles to be added to the map after each iteration.
    /// </summary>
    private List<tile> tempTileList = new List<tile>();
    /// <summary>
    /// A list of possible tiles to add which fit certain conditions. A tile will be randomly selected from this list.
    /// </summary>
    private List<GameObject> TempPossibleTileList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        /// Sorts all the tiles into lists specifying which connections they have.
        StoreTiles();
        /// Bool to make sure the program only adds tiles while new tiles can be added.
        bool changed = true;
        /// Loops until the dungeon has at least the minimum number of rooms.
        do {
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Tile"))
            {
                Destroy(o);
            }
            tileList.Clear();
            tempTileList.Clear();
            connectionList.Clear();
            northConnection.Clear();
            eastConnection.Clear();
            southConnection.Clear();
            westConnection.Clear();
            map = new int[50, 50];
            claimed = new tile[50, 50];
            StoreTiles();
            StartTile();
            changed = true;
            Debug.Log("Here");
            /// Loops while the map size is less than the minimum number of desired tiles.
            while (tileList.Count < length && changed)
                {
                    /// No tile has been changed yet.
                    changed = false;
                    /// Loops through every tile in the current game.
                    foreach (tile itile in tileList)
                    {
                        /// Checks if every available exit of a room is connected to another room.
                        if (!itile.full)
                        {
                            /// Connects tiles to the room if it has exits which lead nowhere.
                            LoadSurroundingTiles(itile);
                            /// Tile has been added so loop again.
                            changed = true;
                        }
                    }
                    /// Adds the tiles which were just added to the list of all tiles.
                    foreach (tile itile in tempTileList)
                    {
                        tileList.Add(itile);
                    }
                    /// Resets the list of temp tiles.
                    tempTileList.Clear();
                }
        }while (tileList.Count < length);
        changed = true;
        while (changed)
        {
            changed = false;
            foreach (tile itile in tileList)
            {
                /// Checks if every available exit of a room is connected to another room.
                if (!itile.full)
                {
                    /// Adds dead end rooms to every available unconnected room left.
                    FinishTiles(itile);
                    /// Tile has been added so loop again.
                    changed = true;
                }
            }
        }

        /// Bool to make sure the program only adds tiles while new tiles can be added.
        /*changed = true;
        while (changed)
        {
            /// No tile has been changed yet.
            changed = false;
            /// Loops through every tile in the current game.
            foreach (tile itile in tileList)
            {
                /// Checks if every available exit of a room is connected to another room.
                if (!itile.full)
                {
                    /// Adds dead end rooms to every available unconnected room left.
                    FinishTiles(itile);
                    /// Tile has been added so loop again.
                    changed = true;
                }
            }
        }*/
        /// Generates the end location in the last tile spawned.
        Instantiate(end,tileList[tileList.Count - 1].worldPosition, Quaternion.identity);
    }
    private void StartTile()
    {
        start = mapTiles[Random.Range(0, mapTiles.Length - 1)];
        /// Selects a random tile from the list of all tiles.
        startTile = new tile(start, 25, 25, new Vector3(0, 0, 0), start.name);
        /// Adds the start tile to the list of tiles in the game.
        tileList.Add(startTile);
        /// The start tile is created in the centre of the 'map'.
        map[25, 25] = 1;
        UpdateMap(startTile);
        /// Creates the start tile at the world origin.
        Instantiate(tileList[Random.Range(0, tileList.Count - 1)].type, tileList[0].worldPosition, Quaternion.identity);
    }
    /// <summary>
    /// Adds tiles which meet specific conditions to every available exit of the tile passed in.
    /// </summary>
    /// <param name="currentTile">The tile which needs rooms added to its exits.</param>
    private void LoadSurroundingTiles(tile currentTile)
    {
        /// Checks if the room has an exit to the north and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[0] == '1' && claimed[currentTile.mapX,currentTile.mapY - 1] == currentTile && map[currentTile.mapX, currentTile.mapY - 1] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && claimed[currentTile.mapX, currentTile.mapY - 2] == null && map[currentTile.mapX, currentTile.mapY - 2] == 0 && claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
            {
                /// Selects a random room with a connection to the south.
                tempTile = connectionList[2][Random.Range(0, connectionList[2].Count - 1)];
                /// Positions the tile correctly.
                tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - 500);
                /// Spawns the new tile.
                Instantiate(tempTile, tempPos, Quaternion.identity);
                /// Creates the tile to be added.
                temp = new tile(tempTile, currentTile.mapX, currentTile.mapY - 1, tempPos, tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                map[temp.mapX, temp.mapY] = 1;
                UpdateMap(temp);
                /// Adds the newly created tile to the array of temp tiles.
                tempTileList.Add(temp);
            }
            else
            {
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && claimed[currentTile.mapX, currentTile.mapY - 2] == null && map[currentTile.mapX, currentTile.mapY - 2] == 0 && claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (claimed[currentTile.mapX, currentTile.mapY - 2] != null || map[currentTile.mapX, currentTile.mapY - 2] == 1) && claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && claimed[currentTile.mapX, currentTile.mapY - 2] == null && map[currentTile.mapX, currentTile.mapY - 2] == 0 && (claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (claimed[currentTile.mapX, currentTile.mapY - 2] != null || map[currentTile.mapX, currentTile.mapY - 2] == 1) && claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0)
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && claimed[currentTile.mapX, currentTile.mapY - 2] == null && map[currentTile.mapX, currentTile.mapY - 2] == 0 && (claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (claimed[currentTile.mapX, currentTile.mapY - 2] != null || map[currentTile.mapX, currentTile.mapY - 2] == 1) && (claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
                {
                    foreach (GameObject itile in connectionList[2])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (claimed[currentTile.mapX, currentTile.mapY - 2] != null || map[currentTile.mapX, currentTile.mapY - 2] == 1) && (claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1))
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
                UpdateMap(temp);
                tempTileList.Add(temp);
                TempPossibleTileList.Clear();
            }
            //UpdateMap(currentTile, 'N');

        }
        /// Checks if the room has an exit to the east and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[1] == '1' && claimed[currentTile.mapX + 1, currentTile.mapY] == currentTile && map[currentTile.mapX + 1, currentTile.mapY] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && claimed[currentTile.mapX + 2, currentTile.mapY] == null && map[currentTile.mapX + 2, currentTile.mapY] == 0 && claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
            {
                /// Selects a random room with a connection to the west.
                tempTile = connectionList[3][Random.Range(0, connectionList[3].Count - 1)];
                /// Positions the tile correctly.
                tempPos = new Vector3(currentTile.worldPosition.x - 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
                /// Spawns the new tile.
                Instantiate(tempTile, tempPos, Quaternion.identity);
                /// Creates the tile to be added.
                temp = new tile(tempTile, currentTile.mapX + 1, currentTile.mapY, tempPos, tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                map[temp.mapX, temp.mapY] = 1;
                UpdateMap(temp);
                /// Adds the newly created tile to the array of temp tiles.
                tempTileList.Add(temp);
            }
            else
            {
                /// Loops through every possible arrangement of surrounding rooms of the tile to be instantiated which would block a connection.
                /// It then randomly selects a tile to be added which fits the conditions.
                if ((claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && claimed[currentTile.mapX + 2, currentTile.mapY] == null && map[currentTile.mapX + 2, currentTile.mapY] == 0 && claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && (claimed[currentTile.mapX + 2, currentTile.mapY] != null || map[currentTile.mapX + 2, currentTile.mapY] == 1) && claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && claimed[currentTile.mapX + 2, currentTile.mapY] == null && map[currentTile.mapX + 2, currentTile.mapY] == 0 && (claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && (claimed[currentTile.mapX + 2, currentTile.mapY] != null || map[currentTile.mapX + 2, currentTile.mapY] == 1) && claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0)
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && claimed[currentTile.mapX + 2, currentTile.mapY] == null && map[currentTile.mapX + 2, currentTile.mapY] == 0 && (claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[0] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX + 1, currentTile.mapY - 1] == null && map[currentTile.mapX + 1, currentTile.mapY - 1] == 0 && (claimed[currentTile.mapX + 2, currentTile.mapY] != null || map[currentTile.mapX + 2, currentTile.mapY] == 1) && (claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
                    foreach (GameObject itile in connectionList[3])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX + 1, currentTile.mapY - 1] != null || map[currentTile.mapX + 1, currentTile.mapY - 1] == 1) && (claimed[currentTile.mapX + 2, currentTile.mapY] != null || map[currentTile.mapX + 2, currentTile.mapY] == 1) && (claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1))
                {
                    Debug.Log("Gothere");
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
                UpdateMap(temp);
                tempTileList.Add(temp);
                TempPossibleTileList.Clear();
            }

        }
        /// Checks if the room has an exit to the south and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[2] == '1' && claimed[currentTile.mapX, currentTile.mapY + 1] == currentTile && map[currentTile.mapX, currentTile.mapY + 1] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && claimed[currentTile.mapX, currentTile.mapY + 2] == null && map[currentTile.mapX, currentTile.mapY + 2] == 0 && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
            {
                /// Selects a random room with a connection to the north.
                tempTile = connectionList[0][Random.Range(0, connectionList[0].Count - 1)];
                /// Positions the tile correctly.
                tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + 500);
                /// Spawns the new tile.
                Instantiate(tempTile, tempPos, Quaternion.identity);
                /// Creates the tile to be added.
                temp = new tile(tempTile, currentTile.mapX, currentTile.mapY + 1, tempPos, tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                map[temp.mapX, temp.mapY] = 1;
                UpdateMap(temp);
                /// Adds the newly created tile to the array of temp tiles.
                tempTileList.Add(temp);
            }
            else
            {
                /// Loops through every possible arrangement of surrounding rooms of the tile to be instantiated which would block a connection.
                /// It then randomly selects a tile to be added which fits the conditions.
                if ((claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && claimed[currentTile.mapX, currentTile.mapY + 2] == null && map[currentTile.mapX, currentTile.mapY + 2] == 0 && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && (claimed[currentTile.mapX, currentTile.mapY + 2] != null || map[currentTile.mapX, currentTile.mapY + 2] == 1) && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && claimed[currentTile.mapX, currentTile.mapY + 2] == null && map[currentTile.mapX, currentTile.mapY + 2] == 0 && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && (claimed[currentTile.mapX, currentTile.mapY + 2] != null || map[currentTile.mapX, currentTile.mapY + 2] == 1) && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && claimed[currentTile.mapX, currentTile.mapY + 2] == null && map[currentTile.mapX, currentTile.mapY + 2] == 0 && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[1] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX + 1, currentTile.mapY + 1] == null && map[currentTile.mapX + 1, currentTile.mapY + 1] == 0 && (claimed[currentTile.mapX, currentTile.mapY + 2] != null || map[currentTile.mapX, currentTile.mapY + 2] == 1) && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in connectionList[0])
                    {
                        if (itile.name[2] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX + 1, currentTile.mapY + 1] != null || map[currentTile.mapX + 1, currentTile.mapY + 1] == 1) && (claimed[currentTile.mapX, currentTile.mapY + 2] != null || map[currentTile.mapX, currentTile.mapY + 2] == 1) && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
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
                UpdateMap(temp);
                TempPossibleTileList.Clear();
            }
        }
        /// Checks if the room has an exit to the west and whether instantiating a tile to the north would leave the map boundaries.
        if (currentTile.config[3] == '1' && claimed[currentTile.mapX - 1, currentTile.mapY] == currentTile && map[currentTile.mapX - 1, currentTile.mapY] != 1)
        {
            /// Checks if there are no rooms which would border the newly instantiated room.
            if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && claimed[currentTile.mapX - 2, currentTile.mapY] == null && map[currentTile.mapX - 2, currentTile.mapY] == 0 && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
            {
                /// Selects a random room with a connection to the east.
                tempTile = connectionList[1][Random.Range(0, connectionList[1].Count - 1)];
                /// Positions the tile correctly.
                tempPos = new Vector3(currentTile.worldPosition.x + 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
                /// Spawns the new tile.
                Instantiate(tempTile, tempPos, Quaternion.identity);
                /// Creates the tile to be added.
                temp = new tile(tempTile, currentTile.mapX - 1, currentTile.mapY, tempPos, tempTile.name);
                /// Updates the map to say that a tile now exists in the current coordinate
                map[temp.mapX, temp.mapY] = 1;
                UpdateMap(temp);
                /// Adds the newly created tile to the array of temp tiles.
                tempTileList.Add(temp);
            }
            else
            {
                /// Loops through every possible arrangement of surrounding rooms of the tile to be instantiated which would block a connection.
                /// It then randomly selects a tile to be added which fits the conditions.
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && claimed[currentTile.mapX - 2, currentTile.mapY] == null && map[currentTile.mapX - 2, currentTile.mapY] == 0 && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (claimed[currentTile.mapX - 2, currentTile.mapY] != null || map[currentTile.mapX - 2, currentTile.mapY] == 1) && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && claimed[currentTile.mapX - 2, currentTile.mapY] == null && map[currentTile.mapX - 2, currentTile.mapY] == 0 && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (claimed[currentTile.mapX - 2, currentTile.mapY] != null || map[currentTile.mapX - 2, currentTile.mapY] == 1) && claimed[currentTile.mapX - 1, currentTile.mapY + 1] == null && map[currentTile.mapX - 1, currentTile.mapY + 1] == 0)
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[3] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && claimed[currentTile.mapX - 2, currentTile.mapY] == null && map[currentTile.mapX - 2, currentTile.mapY] == 0 && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[0] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if (claimed[currentTile.mapX - 1, currentTile.mapY - 1] == null && map[currentTile.mapX - 1, currentTile.mapY - 1] == 0 && (claimed[currentTile.mapX - 2, currentTile.mapY] != null || map[currentTile.mapX - 2, currentTile.mapY] == 1) && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
                {
                    foreach (GameObject itile in connectionList[1])
                    {
                        if (itile.name[3] != '1' && itile.name[2] != '1')
                        {
                            TempPossibleTileList.Add(itile);
                        }
                    }
                }
                if ((claimed[currentTile.mapX - 1, currentTile.mapY - 1] != null || map[currentTile.mapX - 1, currentTile.mapY - 1] == 1) && (claimed[currentTile.mapX - 2, currentTile.mapY] != null || map[currentTile.mapX - 2, currentTile.mapY] == 1) && (claimed[currentTile.mapX - 1, currentTile.mapY + 1] != null || map[currentTile.mapX - 1, currentTile.mapY + 1] == 1))
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
                UpdateMap(temp);
                TempPossibleTileList.Clear();
            }
        }
        /// After every possible situation is checked, the current tile is marked as full.
        currentTile.full = true;
    }
    /// <summary>
    /// Runs on the final iteration. Adds dead end rooms to any room which still has exits with no joining tile.
    /// </summary>
    /// <param name="currentTile">The tile which is being checked to see if it has unboardered exits.</param>
    private void FinishTiles(tile currentTile)
    {
        /// Checks if the tile has a north exit.
        if (currentTile.config[0] == '1' && map[currentTile.mapX,currentTile.mapY - 1] != 1)
        {
            /// Adds a dead end tile with a south connection.
            tempTile = justSouth;
            /// Positions the tile correctly.
            tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z - 500);
            /// Spawns the new tile.
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX, currentTile.mapY - 1, tempPos, tempTile.name);
        }
        /// Checks if the tile has a east exit.
        if (currentTile.config[1] == '1' && map[currentTile.mapX + 1, currentTile.mapY] != 1)
        {
            /// Adds a dead end tile with a west connection.
            tempTile = justWest;
            /// Positions the tile correctly.
            tempPos = new Vector3(currentTile.worldPosition.x - 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
            /// Spawns the new tile.
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX + 1, currentTile.mapY, tempPos, tempTile.name);
        }
        /// Checks if the tile has a south exit.
        if (currentTile.config[2] == '1' && map[currentTile.mapX, currentTile.mapY + 1] != 1)
        {
            /// Adds a dead end tile with a north connection.
            tempTile = justNorth;
            /// Positions the tile correctly.
            tempPos = new Vector3(currentTile.worldPosition.x, currentTile.worldPosition.y, currentTile.worldPosition.z + 500);
            /// Spawns the new tile.
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX, currentTile.mapY + 1, tempPos, tempTile.name);
        }
        /// Checks if the tile has a west exit.
        if (currentTile.config[3] == '1' && map[currentTile.mapX - 1, currentTile.mapY ] != 1)
        {
            /// Adds a dead end tile with a east connection.
            tempTile = justEast;
            /// Positions the tile correctly.
            tempPos = new Vector3(currentTile.worldPosition.x + 500, currentTile.worldPosition.y, currentTile.worldPosition.z);
            /// Spawns the new tile.
            Instantiate(tempTile, tempPos, Quaternion.identity);
            temp = new tile(tempTile, currentTile.mapX - 1, currentTile.mapY, tempPos, tempTile.name);
        }
        /// After every possible situation is checked, the current tile is marked as full.
        currentTile.full = true;
    }
    private void UpdateMap(tile currentTile)
    {
        /*if (type == 'N' && claimed[currentTile.mapX, currentTile.mapY - 1] == null)
        {
            claimed[currentTile.mapX, currentTile.mapY - 1] = currentTile;
        }
        if (type == 'E' && claimed[currentTile.mapX + 1, currentTile.mapY] == null)
        {
            claimed[currentTile.mapX + 1, currentTile.mapY] = currentTile;
        }
        if (type == 'S' && claimed[currentTile.mapX, currentTile.mapY + 1] == null)
        {
            claimed[currentTile.mapX, currentTile.mapY + 1] = currentTile;
        }
        if (type == 'W' && claimed[currentTile.mapX - 1, currentTile.mapY] == null)
        {
            claimed[currentTile.mapX - 1, currentTile.mapY] = currentTile;
        }
        if (type == 'A')
        {*/
            if (currentTile.config[0] == '1' && claimed[currentTile.mapX, currentTile.mapY - 1] == null && map[currentTile.mapX, currentTile.mapY - 1] != 1)
            {
                claimed[currentTile.mapX, currentTile.mapY - 1] = currentTile;
            }
            if (currentTile.config[1] == '1' && claimed[currentTile.mapX + 1, currentTile.mapY] == null && map[currentTile.mapX + 1, currentTile.mapY] != 1)
            {
                claimed[currentTile.mapX + 1, currentTile.mapY] = currentTile;
            }
            if (currentTile.config[2] == '1' && claimed[currentTile.mapX, currentTile.mapY + 1] == null && map[currentTile.mapX, currentTile.mapY + 1] != 1)
            {
                claimed[currentTile.mapX, currentTile.mapY + 1] = currentTile;
            }
            if (currentTile.config[3] == '1' && claimed[currentTile.mapX - 1, currentTile.mapY] == null && map[currentTile.mapX - 1, currentTile.mapY] != 1)
            {
                claimed[currentTile.mapX - 1, currentTile.mapY] = currentTile;
            }
        
        //claimed[currentTile.mapX, currentTile.mapY] = currentTile;
    }
    /// <summary>
    /// Sorts the tiles by their binary representation of exits.
    /// </summary>
    private void StoreTiles()
    {
        foreach (GameObject section in mapTiles)
        {
            /// Checks if the tile has an exit to the north.
            if (section.name[0] == '1')
            {
                /// Adds the tile to the list of tiles which have a north exit.
                northConnection.Add(section);
            }
            /// Checks if the tile has an exit to the east.
            if (section.name[1] == '1')
            {
                /// Adds the tile to the list of tiles which have a east exit.
                eastConnection.Add(section);
            }
            /// Checks if the tile has an exit to the south.
            if (section.name[2] == '1')
            {
                /// Adds the tile to the list of tiles which have a south exit.
                southConnection.Add(section);
            }
            /// Checks if the tile has an exit to the west.
            if (section.name[3] == '1')
            {
                /// Adds the tile to the list of tiles which have a west exit.
                westConnection.Add(section);
            }
            /// Adds the list of north connections to the list of all collections.
            connectionList.Add(northConnection);
            /// Adds the list of east connections to the list of all collections.
            connectionList.Add(eastConnection);
            /// Adds the list of south connections to the list of all collections.
            connectionList.Add(southConnection);
            /// Adds the list of west connections to the list of all collections.
            connectionList.Add(westConnection);
        }
    }
    /// <summary>
    /// A tile made up of a visual asset, position, configuration of exits and position in the 2D array of the game map.
    /// </summary>
    private class tile
    {
        /// <summary>
        /// The tile prefab the tile consists of.
        /// </summary>
        public GameObject type;
        /// <summary>
        /// Where the tile is positioned in the game.
        /// </summary>
        public Vector3 worldPosition;
        /// <summary>
        /// binary representation of the configuration of exits the room has.
        /// </summary>
        public string config;
        /// <summary>
        /// The x coordinate of the position the tile has in the 2D array of the game map.
        /// </summary>
        public int mapX;
        /// <summary>
        /// The y coordinate of the position the tile has in the 2D array of the game map.
        /// </summary>
        public int mapY;
        /// <summary>
        /// Whether the room has exits which aren't connected.
        /// </summary>
        public bool full;
        public int counter;
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
}
