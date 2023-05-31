using UnityEngine;

// A struct representing a line in 2D space
public struct PathLine
{
    const float verticalGradient = 1e5f;
    readonly float perpendicularGradient;
    readonly float pathGradient;
    readonly float y_intercept;
    readonly bool approachSide;
    Vector2 linePoint1;
    Vector2 linePoint2;

    // Constructor that takes in two points and calculates the necessary values for the line
    public PathLine(Vector2 linePoint, Vector2 linePerpendicular)
    {
        // Calculate the gradients of the line and its perpendicular line
        float dx = linePoint.x - linePerpendicular.x;
        float dy = linePoint.y - linePerpendicular.y;
        if (dx == 0)
        {
            perpendicularGradient = verticalGradient;
        }
        else
        {
            perpendicularGradient = dy / dx;
        }

        if (perpendicularGradient == 0)
        {
            pathGradient = verticalGradient;
        }
        else
        {
            pathGradient = -1 / perpendicularGradient;
        }

        // Calculate the y-intercept and two points on the line
        y_intercept = linePoint.y - pathGradient * linePoint.x;
        linePoint1 = linePoint;
        linePoint2 = linePoint + new Vector2(1, pathGradient);

        // Determine which side of the line is the "approach" side based on the perpendicular line
        approachSide = false;
        approachSide = GetSide(linePerpendicular);
    }

    // Which side of the line a given point is on
    bool GetSide(Vector2 p)
    {
        return (p.x - linePoint1.x) * (linePoint2.y - linePoint1.y) > (p.y - linePoint1.y) * (linePoint2.x - linePoint1.x);
    }

    // Determines if a given point has crossed the line
    public bool CrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    // Draws the line with Gizmos in the Unity Editor
    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1, 0, pathGradient).normalized;
        Vector3 lineCenter = new Vector3(linePoint1.x, 0, linePoint1.y) + Vector3.up;
        Gizmos.DrawLine(lineCenter - lineDir * length / 2f, lineCenter + lineDir * length / 2f);

    }

    // Calculates the shortest distance from a point to the end of the line
    public float DistanceToEnd(Vector2 p)
    {
        // Calculate the point where a perpendicular line intersects with this line
        float y_interceptPerpendicular = p.y - perpendicularGradient * p.x;
        float intersectX = (y_interceptPerpendicular - y_intercept) / (pathGradient - perpendicularGradient);
        float intersectY = pathGradient * intersectX + y_intercept;

        // Calculate the distance between the two points
        return Vector2.Distance(p, new Vector2(intersectX, intersectY));
    }
}

