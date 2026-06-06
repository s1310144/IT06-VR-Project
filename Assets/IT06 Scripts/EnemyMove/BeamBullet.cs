using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBullet : MonoBehaviour
{
    public int damage = 1;

    public float lifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Terrain") || other.CompareTag("Weapon")) 
        {
            Destroy(gameObject);
        }
    }
}
