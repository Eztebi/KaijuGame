using UnityEngine;

public class Turret : TurretComponent
{
    public override void Shoot()
    {

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        Vector3 direction = (playerTarget.position - spawnPosition).normalized;
        Quaternion spawnRotation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : transform.rotation;

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        if (bulletObj != null && bulletObj.TryGetComponent<Bullet>(out Bullet bullet))
        {
            bullet.Launch();
        }
    }
}
