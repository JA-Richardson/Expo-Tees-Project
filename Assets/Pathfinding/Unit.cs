using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minUpdateTime = .2f;
    const float pathUpdateThreshold = .5f;
    public float unitSpeed = 5f;
    public float unitTurnDist = 1f;
    public float unitTurnSpeed = 1f;
    public float unitStoppingDist = 5f;
    readonly Stopwatch timer = new();
    Transform target;
    Path path;

    void Start()
    {
        //Sets the target to the base
        target = (GameObject.FindGameObjectWithTag("Base")).transform;
        //Requests a path from the path manager
        PathManager.ReqPath(new PathReq(transform.position, target.position, OnPathFound));
        timer.Start();
    }

    void Update()
    {
        //Requests a new path every second - probably too quick
        if (timer.ElapsedMilliseconds > 1000)
        {
            timer.Reset();
            timer.Start();
            PathManager.ReqPath(new PathReq(transform.position, target.position, OnPathFound));
        }

    }
    //Callback method
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        //If a path is found, create a new path and start following it
        if (pathSuccessful && waypoints != null)
        {
            path = new Path(waypoints, transform.position, unitTurnDist, unitStoppingDist);
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
            //UnityEngine.Debug.Log("Path Found");
        }
    }
    //Coroutine for following the path
    IEnumerator FollowPath()
    {
        float speedPercent = 1;
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);
        while (followingPath)
        {
            //Gets the 2D position of the unit
            Vector2 pos2D = new(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].CrossedLine(pos2D))
            {
                //If unit has reached the end, break out of the loops
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }
            if (followingPath)
            {
                //Starts to slow the unit as it reaches the goal
                if (pathIndex >= path.slowDownIndex && unitStoppingDist > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceToEnd(pos2D) / unitStoppingDist);
                    if (speedPercent < 0.01f)
                    {
                        //If unit speed gets too low, classed as completeing path
                        followingPath = false;
                    }
                }
                //Rotates the unit towards the next point in the path
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position); 
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * unitTurnSpeed);
                transform.Translate(unitSpeed * speedPercent * Time.deltaTime * Vector3.forward, Space.Self); 
            }
            yield return null;
        }

    }

    public void OnDrawGizmos()
    {
        path?.DrawWithGizmos();
    }

    //Coroutine for updating path if target moves, not used due to be a tower defense game
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathManager.ReqPath(new PathReq(transform.position, target.position, OnPathFound));
        float sqrMoveThreshol = pathUpdateThreshold * pathUpdateThreshold;
        Vector3 targetPosOld = target.position;
        while (true)
        {
            yield return new WaitForSeconds(minUpdateTime);
            if ((target.position = targetPosOld).sqrMagnitude > sqrMoveThreshol)
            {
                PathManager.ReqPath(new PathReq(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }
}
