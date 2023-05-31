using System;
using System.Collections.Generic;
using UnityEngine;


public class PathFinding : MonoBehaviour
{
    PathGrid grid;
    void Awake()
    {
        grid = GetComponent<PathGrid>();
    }
    //Find the path using the A* algorithm
    public void FindPath(PathReq request, Action<PathResult> callback)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        PathNode startNode = grid.NodeFromWorldPoint(request.pathStart);
        PathNode targetNode = grid.NodeFromWorldPoint(request.pathEnd);
        if (startNode.walkable && targetNode.walkable)
        {
            //Adds the start node to the open set and creates the closed set
            PathHeap<PathNode> openSet = new(grid.MaxSize);
            HashSet<PathNode> closedSet = new();
            openSet.Add(startNode);
            // While there are nodes in the open set
            while (openSet.Count > 0)
            {
                PathNode node = openSet.RemoveFirst(); // Get the node with the lowest fCost
                closedSet.Add(node); // Add the node to the closed set
                if (node == targetNode)
                {
                    pathSuccess = true; // If the node is the target node, the path was found
                    break;
                }
                //Loop through each neighbour node and calculate the cost to each 
                foreach (PathNode neighbour in grid.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    // Calculate the cost to the neighbour node
                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour) + neighbour.movementPenalty;
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        // If the cost is lower than the previous cost or the neighbour node is not in the open set, update the cost and parent node
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        //If a path was found, retrace the path
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        //Calls the callback function with the path and success values
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }
    //Retrace the path from the end node to the start node
    Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }
    //Simplify the path by removing unnecessary waypoints
    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new();
        Vector2 dirOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 dirNew = new(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (dirNew != dirOld)
            {
                waypoints.Add(path[i].worldPosition);
                dirOld = dirNew;
            }
        }
        return waypoints.ToArray();
    }
    //Get the distance between two nodes
    int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

}