using System;
using UnityEngine;

namespace EsteboFracture
{
    public enum EFractureType
    {
        Voronoi,
        Clustered,
        Slicer
    }
    public enum EPointGenerationType
    {
        Random,
    }
    [Serializable]
    public class FractureParameters
    {
        [HideInInspector]public EFractureType fractureType;
        [HideInInspector]public EPointGenerationType generationType;
        [HideInInspector]public int points = 10;
        [HideInInspector] public Material insideMaterial;

        [Header("Visualization")]
        public bool showGizmos = true;
        public Color gizmoColor = Color.yellow;
        public float gizmoRadius = 0.05f;

        [HideInInspector] public VoronoiCell[] cells;
    }
}