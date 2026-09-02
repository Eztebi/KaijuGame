using PrimeTween;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ParticleSystem damageEffect;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
        Instantiate(damageEffect, transform.localPosition, transform.rotation);
        damageEffect.Play();

        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void Die()
    {

        Instantiate(damageEffect, transform.localPosition, transform.rotation);
        damageEffect.Play();
        Tween.Delay(damageEffect.main.duration, () => {Destroy(gameObject);});
        Debug.Log("Character has died.");
    }
}
