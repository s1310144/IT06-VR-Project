using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageController : MonoBehaviour
{
    private bool grabbing = false;

    private bool holding = false;

    //private bool throwing = false;

    private WeaponDamage _weaponDamage;
    private WeaponEnchant _weaponEnchant;
    // Start is called before the first frame update
    void Start()
    {
        _weaponDamage = GetComponent<WeaponDamage>();
        _weaponEnchant = GetComponent<WeaponEnchant>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holding) {_weaponDamage.SetNoDamage();}
        else if (_weaponEnchant.GetIsEnchanted() && grabbing) { _weaponDamage.SetEnchantDamage(); }
        else if (!_weaponEnchant.GetIsEnchanted() && grabbing) { _weaponDamage.SetNoDamage(); }
        else if (!_weaponEnchant.GetIsEnchanted() && !grabbing) { _weaponDamage.SetDefaultDamage(); }
        else { _weaponDamage.SetNoDamage(); }
    }

    public void ActiveGrab()
    {
        grabbing = true;
    }

    public void inActiveGrab()
    {
        grabbing = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Holder"))
        {
            holding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Holder"))
        {
            holding = false;
        }
    }
}
