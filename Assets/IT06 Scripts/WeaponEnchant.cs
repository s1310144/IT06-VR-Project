using UnityEngine;

public class WeaponEnchant : MonoBehaviour
{
    public GameObject fireEffect;
    public AudioClip enchantSound;

    private bool isEnchanted = false;

    private WeaponDamage _weaponDamage;
    private int originalWeaponDamage;

    private void Start()
    {
        _weaponDamage = GetComponent<WeaponDamage>();
        originalWeaponDamage = _weaponDamage.damage;
        if(isEnchanted) _weaponDamage.damage = 0;
    }

    public void EnchantWeapon()
    {
        if (isEnchanted)
        {
            return;
        }

        isEnchanted = true;

        _weaponDamage.damage = originalWeaponDamage;

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

        _weaponDamage.damage = 0;

        if (fireEffect != null)
        {
            fireEffect.SetActive(false);
        }
    }
}
