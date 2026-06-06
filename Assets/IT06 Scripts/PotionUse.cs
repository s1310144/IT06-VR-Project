using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionUse : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int healAmount = 30;

    public AudioClip potionSound;

    public void UsePotion()
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);

            if (potionSound != null)
            {
                AudioSource.PlayClipAtPoint(potionSound, transform.position);
            }

            Debug.Log("Potion used!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("PlayerHealth is not set!");
        }
    }
}