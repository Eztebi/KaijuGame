using UnityEngine;
using PrimeTween;
using Unity.Cinemachine;
class FeetCollisionMessenger : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private ParticleSystem _rumbleVFX;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Damage")]
    [SerializeField] private float _damageMultiplier = 10f;
    [SerializeField] private float _minimumImpactSpeed = .2f;

    public LegStateMachine stateMachine;

    private Rigidbody _rbFeet;

    private void Start()
    {
        _rbFeet = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (stateMachine == null)
        {
            Debug.LogError(
                "MESSENGER ERROR: " + gameObject.name +
                " no tiene StateMachine asignada."
            );

            return;
        }

        Debug.Log("CHOCOOOOOOOOOOOOOOOOOOOOOOOOO");

        stateMachine.OnCollisionEnter(collision);

        if (collision.collider.CompareTag("Ground"))
        {
            ContactPoint contact = collision.contacts[0];

        Tween.ShakeCamera(_camera,strengthFactor: 1.0f,duration: 0.5f,frequency: 10);

        PlayRumbleVFX(contact.point);

        }
        else if(collision.gameObject.GetComponent<HealthComponent>() != null)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 velocity = _rbFeet.GetPointVelocity(contact.point);

            float impactSpeed = Vector3.Dot(-velocity, contact.normal);
            impactSpeed = Mathf.Abs(impactSpeed);
            Debug.Log("Velocidad de impacto: " + impactSpeed);

            Tween.ShakeCamera(_camera,strengthFactor: 1.0f,duration: 0.5f,frequency: 10);

            PlayRumbleVFX(contact.point);

            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();

            if (health != null && impactSpeed >= _minimumImpactSpeed)
            {
                float damage = impactSpeed * _damageMultiplier;

                Debug.Log(
                    $"Golpe a {collision.gameObject.name} | " +
                    $"Impact Speed: {impactSpeed} | " +
                    $"Damage: {damage}"
                );

                health.TakeDamage(damage);
            }
        }
    }

    private void PlayRumbleVFX(Vector3 position)
    {
        _rumbleVFX.transform.position = position;
        _rumbleVFX.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);

        _rumbleVFX.Play();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (stateMachine != null)
        {
            stateMachine.OnCollisionStay(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (stateMachine != null)
        {
            stateMachine.OnCollisionExit(collision);
        }
    }
}