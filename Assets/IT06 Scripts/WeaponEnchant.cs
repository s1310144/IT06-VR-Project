using UnityEngine;

public class WeaponEnchant : MonoBehaviour
{
    public GameObject fireEffect;
    public AudioClip enchantSound;

    public bool isEnchanted = false;

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

    public void RemoveEnchant()
    {
        if (!isEnchanted)
        {
            return;
        }

        isEnchanted = false;

        if (fireEffect != null)
        {
            fireEffect.SetActive(false);
        }

        Debug.Log(gameObject.name + " ‚ÌƒGƒ“ƒ`ƒƒƒ“ƒg‚ğ”‚ª‚µ‚½");
    }

    public bool GetIsEnchanted() { return isEnchanted; }
}
