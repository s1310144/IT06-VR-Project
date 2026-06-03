using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttack : MonoBehaviour
{
    public float waitTime = 1.5f;
    public float spinSpeed = 720f;
    public float spinTime = 2f;

    public float windupAngle = 60f;

    public float windupTime = 0.5f;

    public Collider spinAttackCollider;
    public Collider spiningEnemyCollider;

    private Collider _collider;
    private bool isSpin;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();

        spinAttackCollider.enabled = false;
        spiningEnemyCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Attack(EnemySound sound)
    {
        Quaternion startRot = transform.rotation;

        // ãtï˚å¸Ç÷Ç–ÇÀÇÈ
        Quaternion windupRot = startRot * Quaternion.Euler(0, -Mathf.Sign(spinSpeed) * windupAngle, 0);

        float timer = 0f;

        // ó\îıìÆçÏ
        while (timer < windupTime)
        {
            timer += Time.deltaTime;

            transform.rotation =Quaternion.Slerp(startRot, windupRot, timer / windupTime);

            yield return null;
        }


        // ó\îıìÆçÏ
        yield return new WaitForSeconds(waitTime);

        timer = 0f;
        isSpin = true;

        sound.PlaySpin();

        _collider.enabled = false;
        spinAttackCollider.enabled = true;
        spiningEnemyCollider.enabled = true;

        while (timer < spinTime)
        {
            timer += Time.deltaTime;

            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        isSpin = false;

        _collider.enabled = true;
        spinAttackCollider.enabled = false;
        spiningEnemyCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isSpin)
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();

            if (hp != null)
            {
                hp.TakeDamage(10);
            }
        }
    }
}
