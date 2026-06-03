using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    Renderer _renderer;

    Color _color;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;

        _renderer = GetComponentInChildren<Renderer>();

        if (_renderer != null)
        {
            _color = _renderer.material.color;
        }
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

        StartCoroutine(DamageFlash());

        // HPÇ™0
        if (currentHP == 0)
        {
            Die();
        }
    }

    IEnumerator DamageFlash()
    {
        if (_renderer == null)
        {
            yield break;
        }

        _renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        _renderer.material.color = _color;
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