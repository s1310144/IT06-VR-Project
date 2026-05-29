using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // É_ÉÅÅ[ÉW
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        // HPÇ™0
        if (currentHP == 0)
        {
            Die();
        }
    }

    // âÒïú
    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    // éÄñS
    void Die()
    {
        Destroy(gameObject);
    }
}
