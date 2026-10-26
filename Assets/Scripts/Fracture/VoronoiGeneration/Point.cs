using UnityEngine;

public class Point 
{
    double x;
    double y;
    double z;

    private Vector3 position;
    public Point(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.position = pos;
    }
    public Vector3 Position => position;
}
