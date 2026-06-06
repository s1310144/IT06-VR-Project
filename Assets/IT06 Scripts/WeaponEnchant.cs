using UnityEngine;

public class WeaponEnchant : MonoBehaviour
{
    public GameObject fireEffect;
    public AudioClip enchantSound;

    private bool isEnchanted = false;

    public void EnchantWeapon()
    {
        if (isEnchanted)
        {
            return;
        }

        isEnchanted = true;

        if (fireEffect != null)
        {
            fireEffect.SetActive(true);
        }

        if (enchantSound != null)
        {
            AudioSource.PlayClipAtPoint(enchantSound, transform.position);
        }

        Debug.Log(gameObject.name + " is enchanted with fire!");
    }
}
