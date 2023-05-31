using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int movementPenalty;

    public int gCost;
    public int hCost;
    public PathNode parent;
    int heapIndex;

    public PathNode(bool walkable, Vector3 worldPos, int gridX, int gridY, int penalty)
    {
        this.walkable = walkable;
        worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
        movementPenalty = penalty;
    }
    //Calculate the F cost of the node
    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    //Set and get the heap index
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    //Compares the costs of two nodes
    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}