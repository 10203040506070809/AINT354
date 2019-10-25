using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMeshGen : MonoBehaviour
{
    public SquareGrid squareGrid;
    /// <summary>
    /// List of vertices for mesh rendering.
    /// </summary>
    List<Vector3> vertices;
    /// <summary>
    /// List of triangles for mesh rendering.
    /// </summary>
    List<int> triangles;
    /// <summary>
    /// Takes in the values from DungeonMapGen and passes them into the SquareGrid constructor.
    /// </summary>
    /// <param name="map">The int array m_map which holds the data of whether coordinates are air or walls.</param>
    /// <param name="squareSize">The size of the squares.</param>
    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
        /// Initialises vertices list.
        vertices = new List<Vector3>();
        /// Initialises triangles list.
        triangles = new List<int>();
        /// Loops through every square in squareGrid.
        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }
        ///Creates a new mesh for the map
        Mesh mesh = new Mesh();
        ///Assigns the newly created mesh to the mesh component of MeshFilter.
        GetComponent<MeshFilter>().mesh = mesh;
        ///Converts the list of vertices to an array and assigns it to the mesh.
        mesh.vertices = vertices.ToArray();
        ///Converts the list of triangles to an array and assigns it to the mesh.
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void TriangulateSquare(Square square)
    {
        /// Switch statement based on square configuration (which control nodes are active).
        switch (square.configuration)
        {
            /// cases if 1 control point is active.
            case 0:
                break;

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

            /// cases if 2 control points are active.
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

            /// cases if 3 control points are active.
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

            /// case if 4 control points are active.
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                break;
        }
    }
    /// <summary>
    /// Creates the correct number of triangles based on how many nodes are active in the square.
    /// </summary>
    /// <param name="points">The array of points passed in based on the configuration of the square.</param>
    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        ///Checks how many points are needed for each possible configuration. 3 points need 1 trianlge, 4 points need 2 triangles, 5 points need 3 triangles and so on.
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
    void AssignVertices(Node[] points)
    {
        ///loops through the points of the configuration for the square and assigns each vertex to a list.
        for (int i = 0; i < points.Length; i++)
        {
            /// Checks for points not yet initialised.
            if (points[i].vertexIndex == -1)
            {
                /// Assigns the vertex index to the size of the vertices list. For example, the list will be initialised with 0 values so the first vertexIndex will also be 0.
                points[i].vertexIndex = vertices.Count;
                /// Adds the Vector3 component of the node to the vertices list.
                vertices.Add(points[i].position);
            }
        }
    }
    /// <summary>
    /// Creates a triangle based on the nodes passed through
    /// </summary>
    /// <param name="a">First vertex of the triangle</param>
    /// <param name="b">Second vertex of the triangle</param>
    /// <param name="c">Third vertex of the triangle</param>
    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
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
            /// Loops through every node
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
