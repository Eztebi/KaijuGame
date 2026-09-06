using UnityEngine;

public abstract class TurretComponent : MonoBehaviour
{
    [Header("Turret Properties")]
    protected float shootRange;
    protected float shootCooldown;
    protected float fireTime;
    protected int ammo;
    protected bool isShooting;
    protected Vector3 targetPos;

    [SerializeField] protected GameObject bulletPrefab;

    virtual protected void Shoot()
    {
        if (bulletPrefab != null)
        {
            bulletPrefab.SetActive(false);
        }
    }
    virtual protected void Reloading()
    {

    }
}

