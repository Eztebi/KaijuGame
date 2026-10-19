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
    public class FractureParameters
    {
        public VoronoiCell[] cells;
        public EFractureType type;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}