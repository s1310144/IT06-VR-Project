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

    public float spinWaitShakeRatio = 0.5f;

    public float spinEndWaitTime = 1.0f;

    public Collider spinAttackCollider;
    public Collider spiningEnemyCollider;
    public Collider hitCollider;

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

        // 逆方向へひねるための角度を求める
        Quaternion windupRot = startRot * Quaternion.Euler(0, -windupAngle, 0);

        float timer = 0f;

        // 時間かけてひねる
        while (timer < windupTime)
        {
            timer += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(startRot, windupRot, timer / windupTime);

            yield return null;
        }


        // ひねったあrtの待機時間
        //yield return new WaitForSeconds(waitTime);

        timer = 0f;

        // ひねった後にプルプル震えるようにする。
        while (timer < waitTime)
        {
            timer += Time.deltaTime;

            // プrプル震えるよう角度計算
            float shakeAngle = Mathf.Sin(timer * 40f) * spinWaitShakeRatio;

            transform.rotation =　windupRot * Quaternion.Euler(0, shakeAngle, 0);

            yield return null;
        }


        // スピン開始
        timer = 0f;
        isSpin = true;

        sound.PlaySpin();

        _collider.enabled = false;
        spinAttackCollider.enabled = true;
        spiningEnemyCollider.enabled = true;
        hitCollider.enabled = false;

        while (timer < spinTime)
        {
            timer += Time.deltaTime;

            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

            yield return null;
        }

        isSpin = false;

        _collider.enabled = true;
        spinAttackCollider.enabled = false;
        spiningEnemyCollider.enabled = false;
        hitCollider.enabled = true;

        // 攻撃後の待機時間
        yield return new WaitForSeconds(spinEndWaitTime);
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
