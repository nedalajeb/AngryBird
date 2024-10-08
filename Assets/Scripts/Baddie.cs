using UnityEngine;

public class Baddie : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3;
    [SerializeField] private float _damageThreshold = 0.2f;
    [SerializeField] private GameObject _baddieDeathParticle; // BaddiDeathParticles

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void DamageBaddie(float damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.RemoveBaddie(this); // Ensure GameManager is correctly set up

        Instantiate(_baddieDeathParticle, transform.position, Quaternion.identity);

        Destroy(gameObject); // Destroy the baddie object
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;
        if (impactVelocity > _damageThreshold)
        {
            DamageBaddie(impactVelocity); // Apply the impact as damage
        }
    }
}