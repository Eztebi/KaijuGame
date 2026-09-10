using PrimeTween;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
class FeetCollisionMessenger : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rumbleVFXPrefab;
    private ParticleSystem _rumbleVFXInstance;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Damage")]
    [SerializeField] private float _damageMultiplier = 20f;
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


        stateMachine.OnCollisionEnter(collision);

        if (collision.collider.CompareTag("Ground"))
        {


            Tween.ShakeCamera(_camera, strengthFactor: 1.0f, duration: 0.5f, frequency: 10);

            PlayRumbleVFX(this.transform.position);

        }
        else if (collision.gameObject.GetComponent<HealthComponent>() != null)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 velocity = _rbFeet.GetPointVelocity(contact.point);

            float impactSpeed = Vector3.Dot(-velocity, contact.normal);
            impactSpeed = Mathf.Abs(impactSpeed);
            Debug.Log("Velocidad de impacto: " + impactSpeed);

            Tween.ShakeCamera(_camera, strengthFactor: 1.0f, duration: 0.5f, frequency: 10);

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
        if (!collision.collider.CompareTag("Player")) Tween.ShakeCamera(Camera.main, 5.0f, 0.5f, 5f);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (stateMachine == null)
        {
            Debug.LogError(
                "MESSENGER ERROR: " + gameObject.name +
                " no tiene StateMachine asignada."
            );

            return;
        }


        stateMachine.OnCollisionExit(collision);
    }

    private void PlayRumbleVFX(Vector3 position)
    {
        if (_rumbleVFXInstance == null)
        {
            if (_rumbleVFXPrefab == null)
            {
                Debug.LogWarning("No hay prefab asignado en _rumbleVFXPrefab.");
                return;
            }
            _rumbleVFXInstance = Instantiate(_rumbleVFXPrefab, position, this.transform.rotation);
        }
        else
        {
            _rumbleVFXInstance.transform.position = position;
            _rumbleVFXInstance.transform.rotation = this.transform.rotation;
            _rumbleVFXInstance.Stop();
        }
        _rumbleVFXInstance.Play();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (stateMachine != null)
        {
            stateMachine.OnCollisionStay(collision);
        }
    }

}