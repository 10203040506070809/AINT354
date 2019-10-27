using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMeshGen : MonoBehaviour
{
    public SquareGrid m_squareGrid;
    /// <summary>
    /// List of vertices for mesh rendering.
    /// </summary>
    private List<Vector3> m_vertices;
    /// <summary>
    /// List of triangles for mesh rendering.
    /// </summary>
    private List<int> m_triangles;
    /// <summary>
    /// A dictionary which contains lists of triangle which share a common vertex.
    /// </summary>
    private Dictionary<int, List<Triangle>> m_triangleDictionary = new Dictionary<int, List<Triangle>>();
    /// <summary>
    /// A list of outlines. Each inner list contains the indeces of the vertices which make up the outline.
    /// </summary>
    private List<List<int>> m_outlines = new List<List<int>>();
    /// <summary>
    /// A list of vertices which have been checked whether they are part of an outline or not.
    /// </summary>
    private HashSet<int> m_checkedVertices = new HashSet<int>();
    public MeshFilter m_walls;
    /// <summary>
    /// Takes in the values from DungeonMapGen and passes them into the SquareGrid constructor.
    /// </summary>
    /// <param name="map">The int array m_map which holds the data of whether coordinates are air or walls.</param>
    /// <param name="squareSize">The size of the squares.</param>
    public void GenerateMesh(int[,] map, float squareSize)
    {
        /// Clears these variables upon each new map generation.
        m_triangleDictionary.Clear();
        m_outlines.Clear();
        m_checkedVertices.Clear();
        m_squareGrid = new SquareGrid(map, squareSize);
        /// Initialises vertices list.
        m_vertices = new List<Vector3>();
        /// Initialises triangles list.
        m_triangles = new List<int>();
        /// Loops through every square in squareGrid.
        for (int x = 0; x < m_squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < m_squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(m_squareGrid.squares[x, y]);
            }
        }
        /// Creates a new mesh for the map.
        Mesh mesh = new Mesh();
        /// Assigns the newly created mesh to the mesh component of MeshFilter.
        GetComponent<MeshFilter>().mesh = mesh;
        /// Converts the list of vertices to an array and assigns it to the mesh.
        mesh.vertices = m_vertices.ToArray();
        /// Converts the list of triangles to an array and assigns it to the mesh.
        mesh.triangles = m_triangles.ToArray();
        mesh.RecalculateNormals();

        CreateWallMesh();
    }
    /// <summary>
    /// Calls the necessary function to generate wall outlines then adds the vertices which make up the outlines to lists which will be fed into the mesh renderer.
    /// </summary>
    private void CreateWallMesh()
    {
        CalculateMeshOutlines();
        /// List of vertices which will make up the wall mesh.
        List<Vector3> wallVertices = new List<Vector3>();
        /// List of triangles which will make up the wall mesh.
        List<int> wallTriangles = new List<int>();
        /// Creates new mesh for the wall.
        Mesh wallMesh = new Mesh();
        /// Set wall height.
        float wallHeight = 5;
        /// Loops through each list of vertices in outlines.
        foreach (List<int> outline in m_outlines)
        {
            /// Loops through each vertex in the list.
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;
                /// Adds the vertices to the lists which will make up the wall mesh.
                wallVertices.Add(m_vertices[outline[i]]); // left
                wallVertices.Add(m_vertices[outline[i + 1]]); // right
                wallVertices.Add(m_vertices[outline[i]] - Vector3.up * wallHeight); // bottom left
                wallVertices.Add(m_vertices[outline[i + 1]] - Vector3.up * wallHeight); // bottom right

                wallTriangles.Add(startIndex + 0);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);
            }
        }
        /// Adds the necessary arrays to the mesh.
        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        m_walls.mesh = wallMesh;
    }
    /// <summary>
    /// Takes in the configuration of the square and defines triangles which will make up the mesh for the square.
    /// </summary>
    /// <param name="square">The square's configuration to be represented as triangles.</param>
    private void TriangulateSquare(Square square)
    {
        /// Switch statement based on square configuration (which control nodes are active).
        switch (square.configuration)
        {
            /// Case if 0 control points are active.
            case 0:
                break;
            /// Cases if 1 control point is active.
            case 1:
                MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                break;
            case 2:
                MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
                break;
            case 4:
                MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
                break;
            case 8:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                break;

            /// Cases if 2 control points are active.
            case 3:
                MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 6:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                break;
            case 9:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                break;
            case 12:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                break;
            case 5:
                MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
                break;
            case 10:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            /// Cases if 3 control points are active.
            case 7:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            /// Case if 4 control points are active.
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                /// Forms a solid wall so will not be outline vertices.
                m_checkedVertices.Add(square.topLeft.vertexIndex);
                m_checkedVertices.Add(square.topRight.vertexIndex);
                m_checkedVertices.Add(square.bottomRight.vertexIndex);
                m_checkedVertices.Add(square.bottomLeft.vertexIndex);
                break;
        }
    }
    /// <summary>
    /// Creates the correct number of triangles based on how many nodes are active in the square.
    /// </summary>
    /// <param name="points">The array of points passed in based on the configuration of the square.</param>
    private void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        /// Checks how many points are needed for each possible configuration. 3 points need 1 trianlge, 4 points need 2 triangles, 5 points need 3 triangles and so on.
        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }
    /// <summary>
    /// Assigns the vertex index of the node based on how many vertices have already been assigned. Also adds the position variable of the node to the vertices list.
    /// </summary>
    /// <param name="points">The array of points passed in based on the configuration of the square.</param>
    private void AssignVertices(Node[] points)
    {
        /// Loops through the points of the configuration for the square and assigns each vertex to a list.
        for (int i = 0; i < points.Length; i++)
        {
            /// Checks for points not yet initialised.
            if (points[i].vertexIndex == -1)
            {
                /// Assigns the vertex index to the size of the vertices list. For example, the list will be initialised with 0 values so the first vertexIndex will also be 0.
                points[i].vertexIndex = m_vertices.Count;
                /// Adds the Vector3 component of the node to the vertices list.
                m_vertices.Add(points[i].position);
            }
        }
    }
    /// <summary>
    /// Creates a triangle based on the nodes passed through.
    /// </summary>
    /// <param name="a">First vertex of the triangle.</param>
    /// <param name="b">Second vertex of the triangle.</param>
    /// <param name="c">Third vertex of the triangle.</param>
    private void CreateTriangle(Node a, Node b, Node c)
    {
        m_triangles.Add(a.vertexIndex);
        m_triangles.Add(b.vertexIndex);
        m_triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }
    /// <summary>
    /// Creates lists of triangles which contain a given vertex.
    /// </summary>
    /// <param name="vertexIndexKey">One vertex of the triangle.</param>
    /// <param name="triangle">The triangle the vertex belongs to.</param>
    private void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        /// If the vertex has already been identified, it adds the new triangle to list which contains the given vertex.
        if (m_triangleDictionary.ContainsKey(vertexIndexKey))
        {
            m_triangleDictionary[vertexIndexKey].Add(triangle);
        }
        /// If the vertex has not already been identified, it adds the new triangle to a new list which contains the given vertex.
        else
        {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            m_triangleDictionary.Add(vertexIndexKey, triangleList);
        }
    }
    /// <summary>
    /// Checks every vertex to see if it is part of an outline. If it is, it then traces the outline and populates the outlines list with the vertices which make up the outline.
    /// </summary>
    private void CalculateMeshOutlines()
    {
        /// Loops through every vertex in the vertices list.
        for (int vertexIndex = 0; vertexIndex < m_vertices.Count; vertexIndex++)
        {
            /// Checks whether the vertex has been verified to be part of an outline.
            if (!m_checkedVertices.Contains(vertexIndex))
            {
                /// Passes in the vertex to see if it's part of an outline.
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                /// Checks whether the vertex is part of an outline.
                if (newOutlineVertex != -1)
                {
                    /// The vertex has no been checked so is added to the list of checked vertices.
                    m_checkedVertices.Add(vertexIndex);
                    /// The vertex will be part of an outline consisting of multiple other vertices so a list containing these vertices is created.
                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    /// Adds the list to outlines.
                    m_outlines.Add(newOutline);
                    /// Recursive function to list each vertex in an outline.
                    FollowOutline(newOutlineVertex, m_outlines.Count - 1);
                    m_outlines[m_outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }
    /// <summary>
    /// Recursive function to list each vertex in an outline.
    /// </summary>
    /// <param name="vertexIndex">The vertex to be checked and possibly added to the list of outline vertices.</param>
    /// <param name="outlineIndex">Which outline in 'outlines' is being traced by the function.</param>
    private void FollowOutline(int vertexIndex, int outlineIndex)
    {
        /// Adds the vertex to its corresponding outline.
        m_outlines[outlineIndex].Add(vertexIndex);
        /// Adds the vertex to the hashset of checked vertices.
        m_checkedVertices.Add(vertexIndex);
        /// Checks if the next connected vertex is part of the outline.
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);
        /// Function calls itself until the outline is complete.
        if (nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }
    /// <summary>
    /// Finds all triangles containing a given vertex then loops through these to see if any of the connected vertices form an edge to the map.
    /// </summary>
    /// <param name="vertexIndex">The triangle vertex being tested.</param>
    /// <returns>Returns the corresponding vertex if it forms an edge or returns -1 if it doesn't.</returns>
    private int GetConnectedOutlineVertex(int vertexIndex)
    {
        /// Creates a list of triangle which contain the passed in vertex.
        List<Triangle> trianglesContainingVertex = m_triangleDictionary[vertexIndex];
        /// Loops through every triangle which contains the passed in vertex.
        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            /// A triangle in the list above.
            Triangle triangle = trianglesContainingVertex[i];
            /// Loops through each vertex in the triangle to see if they are shared by any other triangles and, in turn, whether they form an edge.
            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];
                /// Check to avoid comparing the vertex against itself.
                if (vertexB != vertexIndex && !m_checkedVertices.Contains(vertexB))
                {
                    if (IsOutlineEdge(vertexIndex, vertexB))
                    {
                        /// Returns the vertex which forms an outline edge with the passed in vertex.
                        return vertexB;
                    }
                }
            }
        }
        /// Returns -1 if no outline edge is found.
        return -1;
    }
    /// <summary>
    /// Finds every triangle which shares a vertex with vertexA then checks whether any of these triangles also contain vertexB.
    /// </summary>
    /// <param name="vertexA">One vertex of the line being checked.</param>
    /// <param name="vertexB">The second vertex of the line being checked.</param>
    /// <returns>Returns a bool stating whether or not the two parameters form an outline edge.</returns>
    private bool IsOutlineEdge(int vertexA, int vertexB)
    {
        /// List of triangles containing vertexA;
        List<Triangle> trianglesContainingVertexA = m_triangleDictionary[vertexA];
        /// How many triangles share the given vertices.
        int sharedTriangleCount = 0;
        /// Loops through every triangle which contains vertexA.
        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            /// Checks whether the triangles which contain vertexA also contains vertexB.
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                /// Adds 1 to the count if a triangle shares both vertices.
                sharedTriangleCount++;
                /// If the vertices are shared by more than one triangle, the line they form is not an edge.
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }
    /// <summary>
    /// Constructs triangles by storing three vertices which make up the triangle.
    /// </summary>
    private struct Triangle
    {
        /// <summary>
        /// Index of first vertex of the triangle.
        /// </summary>
        public int vertexIndexA;
        /// <summary>
        /// Index of second vertex of the triangle.
        /// </summary>
        public int vertexIndexB;
        /// <summary>
        /// Index of third vertex of the triangle.
        /// </summary>
        public int vertexIndexC;
        /// <summary>
        /// Int array storing the vertices of the triangle.
        /// </summary>
        int[] vertices;

        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;
            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }
        /// <summary>
        /// Returns the vertex in the position of the array of the parameter passed in.
        /// </summary>
        /// <param name="i">The location in the array where the desired vertex of the triangle is stored.</param>
        /// <returns>A vertex of the triangle.</returns>
        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }
        /// <summary>
        /// Checks if a triangle contains a certain vertex.
        /// </summary>
        /// <param name="vertexIndex">The index of the vertex in the list of all vertices to be compared with the index of each vertice of the triangle.</param>
        /// <returns>Whether or not the triangle contains the vertex.</returns>
        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }
    /// <summary>
    /// Class and constructor for an array of Squares.
    /// </summary>
    /// 
    public class SquareGrid
    {
        public Square[,] squares;
        /// <summary>
        /// Constructor for SquareGrid.
        /// </summary>
        /// <param name="map">The int array m_map which holds the data of whether coordinates are air or walls.</param>
        /// <param name="squareSize">The size of the squares.</param>
        public SquareGrid(int[,] map, float squareSize)
        {
            /// The node count for x and y are equal to the length of the respective row/column in the m_map matrix.
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            /// The size of the generated map will be equal to the number of squares that will be generated multiplied by the chosen square size.
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;
            /// 2D array of controlNodes declared based on the width and height of m_map. 
            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
            /// Loops through every node.
            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    /// Every node's position is generated based on the square size.
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    /// The array of ControlNodes is populated using the position defined above, whether or not the node represents a wall and finally the size of the square.
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }
            /// 2D array of squares declared. The number of squares along the width of the map is equal to the number of Xnodes - 1 and the number of squares along the height of the map is equal to Ynodes - 1.
            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            /// Loops through every node apart from the last one as this would generate a square which would be off the map.
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    /// Populates the squares array with squares generated by adjacent controlNodes.
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }
    /// <summary>
    /// Class and constructor for Squares. These squares are equivelent to the map tiles but are shrunken anchored on the map tile's centre coordinate.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// The control nodes monitor whether the tile in each of the neighbouring diagonal tiles are a wall.
        /// </summary>
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        /// <summary>
        /// The nodes of each orthogonal direction from the centre of the square.
        /// </summary>
        public Node centreTop, centreRight, centreBottom, centreLeft;
        /// <summary>
        /// Configuration of mesh around each point.
        /// </summary>
        public int configuration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            /// Configuration is set dependent on which surrounding nodes are active.
            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }
    }
    /// <summary>
    /// Class and constructor for nodes surrounding map tiles.
    /// </summary>
    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }
    /// <summary>
    /// Class and constructor for Control nodes surrounding map tiles. These nodes are active if they diagonally border a tile which is a wall.
    /// </summary>
    public class ControlNode : Node
    {
        /// <summary>
        /// Whether or not the Control Node borders a wall.
        /// </summary>
        public bool active;
        /// <summary>
        /// The two nodes the controller node controls.
        /// </summary>
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }

    }
}
