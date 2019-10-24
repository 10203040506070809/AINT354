using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonMeshGen : MonoBehaviour
{
    public SquareGrid squareGrid;
    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
    }
    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
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
