using UnityEngine;
using UnityEngine.Android;

public abstract class CharacterBase : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();

}
