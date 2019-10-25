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
        /// Specifies how wide/long the border will be.
        int borderSize = 5;
        /// Creates a new 2D int array with the first dimension being the map width plus the border size multiplied by 2. This is because the border needs to extend over both the left and right side of the map.
        int[,] borderedMap = new int[m_width + borderSize * 2, m_height + borderSize * 2];
        /// Loops through the borderedMap array
        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                /// Checks whether the values are inside the map.
                if (x >= borderSize && x < m_width + borderSize && y >= borderSize && y < m_height + borderSize)
                {
                    /// Sets the value in borderedMap to whatever value is insidem_map.
                    borderedMap[x, y] = m_map[x - borderSize, y - borderSize];
                }
                else
                {
                    /// If the value is outside the map, borderedMap array is filled with a wall value.
                    borderedMap[x, y] = 1;
                }
            }
        }
        /// Passes the m_map array and the specified square size into the GenerateMesh method.
        DungeonMeshGen meshGen = GetComponent<DungeonMeshGen>();
        meshGen.GenerateMesh(borderedMap, 1);
    }
    /// <summary>
    /// Removes map regions if their size is below a threshold.
    /// </summary>
    void ProcessMap()
    {
        /// Gets a list of regions which are wall types.
        List<List<Coord>> wallRegions = GetRegions(1);
        /// The threshold size which determines whether the region gets removed.
        int wallThresholdSize = 50;
        /// Loops through every region of walls.
        foreach (List<Coord> wallRegion in wallRegions)
        {
            /// checks uf the size of the wall region is smaller than the threshold.
            if (wallRegion.Count < wallThresholdSize)
            {
                /// Changes each tile in the region to air
                foreach (Coord tile in wallRegion)
                {
                    m_map[tile.tileX, tile.tileY] = 0;
                }
            }
        }
        /// Gets a list of regions which are air types.
        List<List<Coord>> roomRegions = GetRegions(0);
        /// The threshold size which determines whether the region gets removed.
        int roomThresholdSize = 50;
        /// Loops through every region of air.
        List<Room> survivingRooms = new List<Room>();
        foreach (List<Coord> roomRegion in roomRegions)
        {
            /// checks uf the size of the air region is smaller than the threshold.
            if (roomRegion.Count < roomThresholdSize)
            {
                /// Changes each tile in the region to air
                foreach (Coord tile in roomRegion)
                {
                    m_map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, m_map));
            }
            /// Sorts rooms by size.
            survivingRooms.Sort();
            /// Largest room is set to main room.
            survivingRooms[0].isMainRoom = true;
            /// Sets the main room as accessible via itself.
            survivingRooms[0].isAccessibleFromMainRoom = true;
            ConnectClosestRooms(survivingRooms);
        }
    }
    /// <summary>
    /// Finds the closest seperate rooms and calls a method which connects them via the shortest route.
    /// </summary>
    /// <param name="allRooms">A list of all air regions(rooms).</param>
    void ConnectClosestRooms(List<Room> allRooms)
    {
        /// Shortest distance between two rooms.
        int bestDistance = 0;
        /// Coordinate for the tile in room A which results in the shortest connection.
        Coord bestTileA = new Coord();
        /// Coordinate for the tile in room B which results in the shortest connection.
        Coord bestTileB = new Coord();
        /// Room which results in the shortest connection to room B.
        Room bestRoomA = new Room();
        /// Room which results in the shortest connection to room A.
        Room bestRoomB = new Room();
        /// Whether a possible connection between two rooms has been found.
        bool possibleConnectionFound = false;
        /// Loops through every room.
        foreach (Room roomA in allRooms)
        {
            /// Resets whether a connection is found for each new room being checked.
            possibleConnectionFound = false;
            /// Loops through every room.
            foreach (Room roomB in allRooms)
            {
                /// Makes sure roomA doesn't try to connect with itself.
                if (roomA == roomB)
                {
                    continue;
                }
                /// Makes sure roomA isn't already connected to roomB.
                if (roomA.IsConnected(roomB))
                {
                    possibleConnectionFound = false;
                    break;
                }
                /// Loops through each edge tile of roomA. 
                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    ///Loops through each edge tile of roomB.
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        /// Sets the current coordinates of the tiles being checked
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        /// Calculates the distance between the two tiles.
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));
                        /// Checks if the new distance found is better than the current best distance.
                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            /// Sets the new found distance to be the best distance.
                            bestDistance = distanceBetweenRooms;
                            /// Updates connection found to true.
                            possibleConnectionFound = true;
                            /// Sets the values needed to identify the connection between the rooms.
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            /// Checks if a connection between rooms has been found.
            if (possibleConnectionFound)
            {
                /// Calls the function to connect the rooms.
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
    }
    /// <summary>
    /// Updates the rooms passed in to say that they're now connected.
    /// </summary>
    /// <param name="roomA">First room to be connected.</param>
    /// <param name="roomB">Second room to be connected.</param>
    /// <param name="tileA">The tile from roomA to be connected to the tile in roomB.</param>
    /// <param name="tileB">The tile from roomB to be connected to the tile in roomA.</param>
    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        /// Updates the rooms to say they're connected to eachother.
        Room.ConnectRooms(roomA, roomB);
        Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100);
    }
    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-m_width / 2 + .5f + tile.tileX, 2, -m_height / 2 + .5f + tile.tileY);
    }

    /// <summary>
    /// Returns a list of regions of the specified tile type (air/wall).
    /// </summary>
    /// <param name="tileType">1 or 0 depending on whether the tile is a wall or air.</param>
    /// <returns>A list of regions containing a list of tiles.</returns>
    List<List<Coord>> GetRegions(int tileType)
    {
        /// Initiates the list of regions.
        List<List<Coord>> regions = new List<List<Coord>>();
        /// 2D array the same size as the map which will store information on whether a tile has been checked.
        int[,] mapFlags = new int[m_width, m_height];
        /// Loops through every tile in the map
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                /// Checks if the tile has been previously checked and if the tile type matches the passed in parameter.
                if (mapFlags[x, y] == 0 && m_map[x, y] == tileType)
                {
                    /// Generates a list of coordinates which make up a region.
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    /// Adds the list of coordinates to the list of regions.
                    regions.Add(newRegion);
                    /// Marks each tile in the region previously discovered as checked.
                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    /// <summary>
    /// Given a starting tile finds all other bordering tiles which share the same type (air/wall)
    /// </summary>
    /// <param name="startX">The X coordinate of the tile being checked.</param>
    /// <param name="startY">The Y coordinate of the tile being checked.</param>
    /// <returns>A list of tile coordinates which define a region of tiles which are the same type.</returns>
    List<Coord> GetRegionTiles(int startX, int startY)
    {
        /// List of coordinates which will define a region.
        List<Coord> tiles = new List<Coord>();
        /// 2D array the same size as the map which will store information on whether a tile has been checked.
        int[,] mapFlags = new int[m_width, m_height];
        /// Determines whether the starting coordinate is a wall or air tile.
        int tileType = m_map[startX, startY];
        /// Creates a queue of coordinates which will be checked to see if they border any other tiles of the same type.
        Queue<Coord> queue = new Queue<Coord>();
        /// Starting coordinate is added to the queue for testing. 
        queue.Enqueue(new Coord(startX, startY));
        /// Coordinate is marked as being checked for belonging to a region.
        mapFlags[startX, startY] = 1;
        /// Loops until no more tiles of the same type are located within a region.
        while (queue.Count > 0)
        {
            /// Removes the current tile from the queue.
            Coord tile = queue.Dequeue();
            /// Adds the tile to the list of tiles in the region
            tiles.Add(tile);
            /// Loop through the surrounding tiles in a 3x3 area.
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    /// Checks that the tile is within the map and excludes checks on diagonal tiles.
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        /// Checks whether the tile has already been checked and whether it is the same tile type as the starting tile.
                        if (mapFlags[x, y] == 0 && m_map[x, y] == tileType)
                        {
                            /// Updates mapFlags to know that the tile has been checked.
                            mapFlags[x, y] = 1;
                            /// Adds all tiles which border the current tile and share the the same type (air/wall) as the starting tile to be checked.
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }
    /// <summary>
    /// Checks whether the passed in coordinates are within the map space.
    /// </summary>
    /// <param name="x">The value of the x coordinate being checked.</param>
    /// <param name="y">The value of the y coordinate being checked.</param>
    /// <returns>Bool showing whether the passed in coordinates are within the map space.</returns>
    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < m_width && y >= 0 && y < m_height;
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
                if (IsInMapRange(neighbourX,neighbourY))
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
    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }
    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room()
        {
        }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (x == tile.tileX || y == tile.tileY)
                        {
                            if (map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }
        public static void ConnectRooms(Room roomA, Room roomB)
        {
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }
        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
        /// <summary>
        /// Draws the matrix m_map in unity using black and white squares. Black squares are walls, white squares are air.
        /// </summary>
        /*void OnDrawGizmos()
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
        }*/
    }
