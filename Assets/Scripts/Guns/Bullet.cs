using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Propiedades de la Bala")]
    [SerializeField] private float speed = 35f;
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody rb;
    private bool isReturned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch()
    {
        isReturned = false; 

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = transform.forward * speed;

        StopAllCoroutines();
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(10f);
        ReturnToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignorar la propia torreta o impactos entre proyectiles
        if (collision.gameObject.CompareTag("Turret") || collision.gameObject.CompareTag("Bullet")) return;

        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignorar la TriggerZone de la torreta u otros triggers del mapa
        if (other.isTrigger) return;

        ReturnToPool();
    }

    public void ReturnToPool()
    {
        if (isReturned) return;
        isReturned = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        Destroy(gameObject);
    }
}