using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    readonly Queue<PathResult> result = new();
    PathFinding pathFinding;
    static PathManager instance;

    void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }
    
    void Update()
    {
        if (result.Count > 0)
        {
            int itemsInQueue = result.Count;
            //Locks queue to prevent other threads from modifying it
            lock (result)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult pathResult = result.Dequeue();
                    //Call the callback function with the path and success values
                    pathResult.callback(pathResult.path, pathResult.success);
                }
            }
        }
    }
    //Starts a thread to find the path
    public static void ReqPath(PathReq request)
    {
        void startThread()
        {
            instance.pathFinding.FindPath(request, instance.FinishedProcessingPath);
        }
        startThread();
    }
    //Adds the path result to the queue
    public void FinishedProcessingPath(PathResult pathResult)
    {
        //Locks queue to prevent other threads from modifying it
        lock (result)
        {
            instance.result.Enqueue(pathResult);
        }
    }
}
//Struct to represent the pathfinding request
public struct PathReq
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathReq(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}
//Struct to represent the pathfinding results
public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;
    
    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

