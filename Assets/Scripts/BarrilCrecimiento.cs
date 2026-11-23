using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class GrowdthPowerUp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Transform parent = collision.collider.transform.root;
            GrowManager growManager = parent.GetComponent<GrowManager>();
            if (growManager)
            {
                growManager.SetScale(growManager.CurrentScale + 1);

                Debug.LogWarning("Escala");
            }
            DestroyOnContact();
        }
    }

    private void DestroyOnContact()
    {
        Destroy(this.gameObject);
    }
}
