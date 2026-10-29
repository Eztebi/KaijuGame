using System;
using UnityEngine;

namespace EsteboFracture
{
    [Serializable]
    public class VoronoiCell
    {
        Point site;
        Point[] vertices;
        Color color;
        public VoronoiCell(Point site)
        {
            this.site = site;
        }
        public Point GetSite => site;
    }
}