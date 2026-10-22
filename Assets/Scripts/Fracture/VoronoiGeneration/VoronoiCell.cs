using UnityEngine;

namespace EsteboFracture
{
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