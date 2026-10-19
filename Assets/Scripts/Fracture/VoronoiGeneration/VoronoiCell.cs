using UnityEngine;

namespace EsteboFracture
{
    public class VoronoiCell
    {
        Point site;
        Point[] vertices;

        public VoronoiCell(Point site)
        {
            this.site = site;
        }
    }
}