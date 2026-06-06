using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public float redHpRatio = 0.2f;

    public Volume volume;

    public AudioSource audioSource;
    public AudioClip damageClip;

    public GameObject gameOverBoard;
    public GameObject enemy;

    private Vignette vignette;
    private ChromaticAberration chromatic;

    private float baseVignette;
    private float baseChromatic;

    private Color defaultColor;

    private bool isFlash = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;

        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromatic);

        baseVignette = vignette.intensity.value;
        baseChromatic = chromatic.intensity.value;

        defaultColor = vignette.color.value;

        if (gameOverBoard != null)
        {
            gameOverBoard.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlash) return;

        if ((float)currentHP / maxHP <= redHpRatio)
        {
            vignette.color.value = Color.red;
            vignette.intensity.value = 0.8f;
            vignette.smoothness.value = 0.8f;
            vignette.active = true;
        }
        else
        {
            vignette.color.value = defaultColor;
            vignette.intensity.value = baseVignette;
            vignette.smoothness.value = baseChromatic;
            vignette.active = true;
        }
    }

    // ƒ_ƒ[ƒW
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        PlayDamage();

        StartCoroutine(Flash());

        // HP‚ª0
        if (currentHP == 0)
        {
            Die();
        }
    }

    IEnumerator Flash()
    {
        isFlash = true;

        vignette.color.value = Color.red;
        vignette.intensity.value = 0.9f;
        vignette.smoothness.value = 0.3f;
        vignette.active = true;

        yield return new WaitForSeconds(0.1f);

        float timer = 0;
        float start = vignette.intensity.value;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 8f;
            vignette.intensity.value = Mathf.Lerp(start, baseVignette, timer);
            yield return null;
        }

        vignette.color.value = defaultColor;
        isFlash = false;
    }

    // ‰ñ•œ
    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    // Ž€–S
    void Die()
    {
        if (gameOverBoard != null)
        {
            enemy.SetActive(false);
            gameOverBoard.SetActive(true);
        }
    }

    public void PlayDamage()
    {
        audioSource.PlayOneShot(damageClip);
    }
}
