using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public SqureGrid squreGrid;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squreGrid = new SqureGrid(map, squareSize);
    }

    public class SqureGrid
    {
        public Square[,] squares;

        public SqureGrid(int[,] map, float squreSize)
        { 
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapwidth = nodeCountX * squreSize;
            float mapheight = nodeCountY * squreSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            { 
                for(int y = 0; y < nodeCountY; y++) 
                {
                    Vector3 pos = new Vector3(-mapwidth / 2 + x * squreSize + squreSize / 2, 0,
                                               -mapheight / 2 + y * squreSize + squreSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squreSize);
                }
            }


        }
    }

    public class Square 
    {
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node centerTop, centerRight, centerLeft, centerBottom;

        public Square(ControlNode _topLeft, ControlNode _topRight,
                     ControlNode _bottomLeft, ControlNode _bottomRight) 
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomLeft = _bottomLeft;
            bottomRight = _bottomRight;

            centerTop = topLeft.right;
            centerRight = bottomRight.above;
            centerBottom = bottomLeft.right;
            centerLeft = bottomLeft.above;
        }
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos) { position = _pos; }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squreSize) : base(_pos)
        { 
            active = _active;
            above = new Node(position + Vector3.forward * squreSize/2f);
            right = new Node(position + Vector3.right * squreSize / 2f);
        }
    }
}
