using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class TurretComponent : MonoBehaviour
{
    [Header("Propiedades de Torreta")]
    [SerializeField] protected float shootCooldown = 1f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected Transform turretHead;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject bulletPrefab;

    [Header("Health Component")]
    [SerializeField] protected HealthComponent healthComponent;
    
    protected float fireTime;
    protected Transform playerTarget;
    protected Transform target;
    protected bool canShoot = false;

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTarget = player.transform;
        }


        if (healthComponent == null)
        {
            healthComponent = GetComponent<HealthComponent>();
        }

        if (turretHead == null)
        {
            turretHead = transform;
        }
    }

    protected virtual void Update()
    {
        if (!canShoot || playerTarget == null || (healthComponent != null && healthComponent.CurrentHealth <= 0)) return;

        LookAtPlayer();

        if (Time.time >= fireTime)
        {
            fireTime = Time.time + shootCooldown;
            Shoot();
        }
    }

    protected virtual void LookAtPlayer()
    {
        Vector3 direction = playerTarget.position - turretHead.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartShooting() => canShoot = true;
    public void StopShooting() => canShoot = false;

    public abstract void Shoot();

    public virtual void TakeDamage(float damage)
    {
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);
        }
    }
}