using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int throwingDamage = 5;
    public int enchantDamage = 20;

    private int currentDamage;

    public int GetDamage() { return currentDamage; }
    public void SetDamage(int d) { currentDamage = d; }
    public void SetDefaultDamage() { currentDamage = throwingDamage; }
    public void SetEnchantDamage() { currentDamage = enchantDamage; }
    public void SetNoDamage() { currentDamage = (int)0; }
}
