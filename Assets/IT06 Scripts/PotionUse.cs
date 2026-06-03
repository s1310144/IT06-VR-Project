using UnityEngine;

public class PotionUse : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int healAmount = 30;

    public void UsePotion()
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log("Potion used!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("PlayerHealth is not set!");
        }
    }
}