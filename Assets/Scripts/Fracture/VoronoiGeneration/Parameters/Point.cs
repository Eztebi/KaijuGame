using System;
using UnityEngine;

[Serializable]
public class Point
{
    private Vector3 position;

    public Point(Vector3 pos)
    {
        this.position = pos;
    }

    public Vector3 Position => position;
}
