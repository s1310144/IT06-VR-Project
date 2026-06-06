using UnityEngine;

public class EnemyHitDetector : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    public AudioClip hitSound;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.Log("EnemyHealth is not found on enemy!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy trigger touched: " + other.gameObject.name);

        WeaponDamage weaponDamage = other.GetComponent<WeaponDamage>();

        if (weaponDamage != null)
        {
            enemyHealth.TakeDamage(weaponDamage.damage);

            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            Debug.Log("Enemy damaged by " + other.gameObject.name);
        }
    }
}