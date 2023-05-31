using UnityEngine;

public class Path
{
    public readonly Vector3[] lookPoints;
    public readonly PathLine[] turnBoundaries;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    
    public Path(Vector3[] waypoint, Vector3 startPos, float turnDist, float stoppingDist)
    {
        lookPoints = waypoint;
        turnBoundaries = new PathLine[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;
        //Converts start pos to a Vector2
        Vector2 prevPoint = V3ToV2(startPos);
        //Loops through the points in order
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(lookPoints[i]); //Converts the current point to a Vector2
            Vector2 dirToCurrentPoint = (currentPoint - prevPoint).normalized; //Gets the direction to the current point
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDist; //Gets the point where the unit will turn
            turnBoundaries[i] = new PathLine(turnBoundaryPoint, prevPoint - dirToCurrentPoint * turnDist); //Creates a new PathLine
            prevPoint = turnBoundaryPoint; //Sets the previous point to the turn boundary point
        }

        float distFromEnd = 0;
        //Loops through the points in reverse order
        for (int i = lookPoints.Length - 1; i > 0; i--)
        {
            //Calculate distance between current point and previousd
            distFromEnd += Vector3.Distance(lookPoints[i], lookPoints[i - 1]);
            
            if (distFromEnd > stoppingDist)
            {
                slowDownIndex = i;
                break;
            }
        }
    }
    //Converts a Vector3 to a Vector2
    Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    // Draws the path in the scene view
    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }
        Gizmos.color = Color.white;
        foreach (PathLine p in turnBoundaries)
        {
            p.DrawWithGizmos(10);
        }

    }

}
