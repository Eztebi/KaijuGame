using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class GrowdthPowerUp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Transform parent = collision.collider.transform.root;
            GrowManager growManager = parent.GetComponentInChildren<GrowManager>();
            growManager.SetScale(growManager.CurrentScale + 1);

            DestroyOnContact();
        }
    }

    private void DestroyOnContact()
    {
        Destroy(this.gameObject);
    }
}
