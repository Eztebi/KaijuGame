using System;
using UnityEngine;

namespace EsteboFracture
{
    public enum EFractureType
    {
        Voronoi,
        Clustered,
    }
    public enum EPointGenerationType
    {
        Random,
    }
    [Serializable]
    public class FractureParameters
    {
        public VoronoiCell[] cells;
        public EFractureType fractureType;
        public EPointGenerationType generationType;
        public int points;
    }
}