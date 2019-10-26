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
        ProcessMap();
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
        }
        /// Sorts rooms by size.
        survivingRooms.Sort();
        /// Largest room is set to main room.
        survivingRooms[0].isMainRoom = true;
        /// Sets the main room as accessible via itself.
        survivingRooms[0].isAccessibleFromMainRoom = true;
        ConnectClosestRooms(survivingRooms);
    }
    /// <summary>
    /// Finds the closest seperate rooms and calls a method which connects them via the shortest route.
    /// </summary>
    /// <param name="allRooms">A list of all air regions(rooms).</param>
    /// <param name="forceAccessibilityFromMainRoom">A bool stating whether a room needs to be forced to connect to the main room.</param>
    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();
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
        /// Checks if the room needs to be forced to connect to the main room
        if (forceAccessibilityFromMainRoom)
        {
            /// Sorts rooms into two lists (those which are accessible from the main room and those which aren't).
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        /// Carries on as normal
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }
        /// Loops through every room.
        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                /// Resets whether a connection is found for each new room being checked.
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            /// Loops through every room.
            foreach (Room roomB in roomListB)
            {
                /// Makes sure roomA doesn't try to connect with itself and roomA isnt already connected to roomB.
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
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
            /// Checks if a connection between rooms has been found and doesnt need to be forced to connect to main room.
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                /// Calls the function to connect the rooms.
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        /// Checks if a connection between rooms has been found and does need to be forced to connect to main room.
        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            /// Calls the function to connect the rooms.
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }
        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
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
        Debug.Log("here");
        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            DrawCircle(c, 5);
        }
    }
    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if (IsInMapRange(drawX, drawY))
                    {
                        m_map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Gets a list of points relating to map tiles which fall on a line between the passed in parameters.
    /// </summary>
    /// <param name="from">The starting coordinate of the line.</param>
    /// <param name="to">The ending coordinate of the line.</param>
    /// <returns>A list of coordinates which fall on the line between the passed in parameters.</returns>
    List<Coord> GetLine(Coord from, Coord to)
    {
        /// The list of coordinates which lie on the line
        List<Coord> line = new List<Coord>();
        /// The x coordinate of the starting point.
        int x = from.tileX;
        /// The y coordinate of the starting point.
        int y = from.tileY;
        /// The change in the x coordinates of the start and end of the line.
        int dx = to.tileX - from.tileX;
        /// The change in the y coordinates of the start and end of the line.
        int dy = to.tileY - from.tileY;
        /// Whether the change in x is greater than the change in y.
        bool inverted = false;
        /// Whether the line moves left or right.
        int step = Math.Sign(dx);
        /// Whether the line moves up or down.
        int gradientStep = Math.Sign(dy);
        /// Used to determine whether the change in x is greater than the change in y.
        int longest = Mathf.Abs(dx);
        /// Used to determine whether the change in x is greater than the change in y.
        int shortest = Mathf.Abs(dy);
        /// Check to see if the change in x is greater than the change in y.
        if (longest < shortest)
        {
            /// Update to say that the change in y is greater than the change in x.
            inverted = true;
            /// Switches the variables below around.
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }
        /// Based on the threshold between map tiles.
        int gradientAccumulation = longest / 2;
        /// Loops through the x or y coordinates depending on which is greater.
        for (int i = 0; i < longest; i++)
        {
            /// Adds a coordinate which falls on the line to the list
            line.Add(new Coord(x, y));
            /// Move along y axis if change in y is greater than change in x.
            if (inverted)
            {
                y += step;
            }
            /// Move along x axis if change in x is greater than change in y.
            else
            {
                x += step;
            }
            /// Adds the change in x or y to the threshold
            gradientAccumulation += shortest;
            /// If the threshold has been passed, add to the corresponding x or y value.
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                /// Resets threshold.
                gradientAccumulation -= longest;
            }
        }

        return line;
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
    /// <summary>
    /// Rooms are defined by any number of connected tiles which share the same tile type (wall/air).
    /// </summary>
    class Room : IComparable<Room>
    {
        /// <summary>
        /// The tiles which make up the room.
        /// </summary>
        public List<Coord> tiles;
        /// <summary>
        /// The tiles which border the room.
        /// </summary>
        public List<Coord> edgeTiles;
        /// <summary>
        /// The rooms connected to the room in question.
        /// </summary>
        public List<Room> connectedRooms;
        /// <summary>
        /// How many tiles make up the room.
        /// </summary>
        public int roomSize;
        /// <summary>
        /// Whether the room is accessible from the main room (largest room).
        /// </summary>
        public bool isAccessibleFromMainRoom;
        /// <summary>
        /// Whether the room is the main room (largest room).
        /// </summary>
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
            /// Checks every tile in the room to see if it borders a wall. If it does then it is added to the edge tiles.
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
        /// <summary>
        /// Adds the rooms passed in to rooms which are connected. If either room is accessible from the main room it updates the other room to being accessible from the main room as well.
        /// </summary>
        /// <param name="roomA">Room to be connected to roomB</param>
        /// <param name="roomB">Room to be connected to roomA</param>
		public static void ConnectRooms(Room roomA, Room roomB)
        {
            /// Checks if roomA is accessible from the main room.
            if (roomA.isAccessibleFromMainRoom)
            {
                /// Adds roomB as accessible from main room.
                roomB.SetAccessibleFromMainRoom();
            }
            /// Checks if roomB is accessible from the main room.
            else if (roomB.isAccessibleFromMainRoom)
            {
                /// Adds roomA as accessible from main room.
                roomA.SetAccessibleFromMainRoom();
            }
            /// Adds roomB as a connected room for roomA.
            roomA.connectedRooms.Add(roomB);
            /// Adds roomA as a connected room for roomB.
            roomB.connectedRooms.Add(roomA);
        }
        /// <summary>
        /// Checks whether the room is connected to another room.
        /// </summary>
        /// <param name="otherRoom">The room to be checked</param>
        /// <returns>Returns true if the room is connected to another room and false if it isn't.</returns>
        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }
        /// <summary>
        /// Compares the sizes of rooms.
        /// </summary>
        /// <param name="otherRoom">The room to be compared.</param>
        /// <returns>Returns an int describing whether the room is larger, smaller or equal to another room.</returns>
        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
        /// <summary>
        /// Sets a room to be accessible from main room. Also sets every connected room to the room in question to acessible from main room too.
        /// </summary>
        public void SetAccessibleFromMainRoom()
        {
            /// Check to see if it isnt already connected to main room.
            if (!isAccessibleFromMainRoom)
            {
                /// Sets the room as accessible from main room.
                isAccessibleFromMainRoom = true;
                /// Sets every connected room as accessible from main room as well.
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
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
