using UnityEngine;

public class EnemyHitDetector : MonoBehaviour
{
    private EnemyHealth enemyHealth;

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
            Debug.Log("Enemy damaged by " + other.gameObject.name);
        }
    }
}