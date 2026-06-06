using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public bool isDead = false;

    public float damageFlashTime = 0.2f; 

    public GameObject middleHpEffects;
    public float middleHpRatio = 0.5f;
    public GameObject lowHpEffects;
    public float lowHpRatio = 0.5f;

    public GameObject explosionPrefab;
    public Transform[] explosionPositions;
    public Transform lastExplosionPosition;
    public float explosionDestroyTime = 1.0f;
    public float deadBodyScale = 0.1f;
    public float deadUpForce = 10.0f;
    public float deadToSmallTime = 1.0f;

    public bool debugKill = false;

    public AudioSource audioSource;
    public AudioClip damageClip;

    public Behaviour restartManager;
    public bool useRestartManager = true;


    private XRGrabInteractable _interactable;

    private Renderer[] _renderers;

    private Color[] originalColors;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;

        _renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
        {
            originalColors[i] = _renderers[i].material.color;
        }

        _interactable = GetComponent<XRGrabInteractable>();
        _interactable.enabled = false;

        if (restartManager != null)
        {
            restartManager.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugKill) 
        {
            currentHP = 0;
        }

        if (!isDead) {
            HpEffect();
        }

        // HPÇ™0
        if (currentHP == 0)
        {
            Die();
        }
    }

    // É_ÉÅÅ[ÉW
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        PlayDamage();

        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = Color.red;
        }

        yield return new WaitForSeconds(damageFlashTime);

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = originalColors[i];
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
        if (isDead) return;
        isDead = true;

        StartCoroutine(Death());
    }

    IEnumerator Death()
    {

        float timer = 0;

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = transform.localScale * deadBodyScale;


        GameObject explosion;

        for (int i = 0; i < explosionPositions.Length; i++)
        {
            explosion = Instantiate(explosionPrefab, explosionPositions[i].position, Quaternion.identity);

            yield return new WaitForSeconds(explosionDestroyTime);

            Destroy(explosion);

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        explosion = Instantiate(explosionPrefab, lastExplosionPosition.position, Quaternion.identity);
        explosion.transform.localScale *= 6f;
        Destroy(explosion, explosionDestroyTime);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(Vector3.up * deadUpForce, ForceMode.Impulse);
        Vector3 torque = new (UnityEngine.Random.Range(-1.0f, 1.0f) * UnityEngine.Random.Range(0f, 5f), 
            UnityEngine.Random.Range(-1.0f, 1.0f) * UnityEngine.Random.Range(0f, 5f), 
            UnityEngine.Random.Range(-1.0f, 1.0f) * UnityEngine.Random.Range(0f, 5f));
        rb.AddTorque(torque, ForceMode.Impulse);

        while (timer < deadToSmallTime)
        {
            timer += Time.deltaTime * 0.8f;

            transform.localScale = Vector3.Lerp(startScale, targetScale, timer);

            yield return null;
        }

        //rb.AddForce(Vector3.up * deadUpForce, ForceMode.Impulse);

        middleHpEffects.SetActive(false);
        lowHpEffects.SetActive(false);


        _interactable.enabled = true;


        if (restartManager != null && useRestartManager == true)
        {
                yield return new WaitForSeconds(2.0f);
                restartManager.enabled = true;
        }
     
    }

    void HpEffect() 
    {
        float hpRatio = (float)currentHP / maxHP;

        if (hpRatio <= middleHpRatio)
        {
            middleHpEffects.SetActive(true);
        }
        else 
        {
            middleHpEffects.SetActive(false);
        }

        if (hpRatio <= lowHpRatio)
        {
            lowHpEffects.SetActive(true);
        }
        else
        {
            lowHpEffects.SetActive(false);
        }
    }

    public void PlayDamage()
    {
        audioSource.PlayOneShot(damageClip);
    }
}