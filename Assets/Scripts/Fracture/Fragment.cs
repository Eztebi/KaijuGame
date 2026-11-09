using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    public float health = 100f;
    public List<Fragment> neighbors = new List<Fragment>();

    public void RecieveImpact(float impactDamage, HashSet<Fragment> processedFragments = null)
    {
        if (processedFragments == null)
        {
            processedFragments = new HashSet<Fragment>();
        }
        if (processedFragments.Contains(this)) return;
        processedFragments.Add(this);
        health -= impactDamage;

        if (health <= 0)
        {
            ActivateRigidbody(); 
            Destroy(gameObject, 20f); 
        }
        else
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i] != null)
                {
                    neighbors[i].RecieveImpact(impactDamage * 0.5f, processedFragments);
                }
            }
        }
    }

    void ActivateRigidbody()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        MeshCollider col = gameObject.GetComponent<MeshCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<MeshCollider>();
            col.convex = false;
        }
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
        rb.useGravity = true;

        neighbors.Clear();
    }
}