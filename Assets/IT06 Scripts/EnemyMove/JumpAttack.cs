using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public float jumpWaitTime = 1.5f;
    public float jumpingTime = 1.5f;
    public GameObject jumpAreaPrefab;

    public float jumpEndWaitTime = 1.0f;

    public float damageRange = 4f;
    public int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Attack(Vector3 targetPos, EnemySound sound)
    {
        //Debug.Log("Jump attack begins / ジャンプ攻撃開始");
        Debug.Log(this.gameObject.name + " Jump attack begins / " + this.gameObject.name + " がジャンプ攻撃開始");

        Vector3 startPos = transform.position;

        // 危険エリア生成
        GameObject warning = Instantiate(jumpAreaPrefab, targetPos + Vector3.up * 0.1f, Quaternion.identity);
        warning.transform.localScale = new Vector3(damageRange * 2, 1, damageRange * 2);

        // 予備動作
        yield return new WaitForSeconds(jumpWaitTime);

        float timer = 0f;

        while (timer < jumpingTime)
        {
            timer += Time.deltaTime;

            float t = timer / jumpingTime;

            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            pos.y += Mathf.Sin(t * Mathf.PI) * 5f;

            transform.position = pos;

            yield return null;
        }

        sound.PlayJump();

        // 着地ダメージ
        Collider[] hitColliders = Physics.OverlapSphere(targetPos, damageRange);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth hp = hit.GetComponent<PlayerHealth>();

                if (hp != null)
                {
                    hp.TakeDamage(damage);
                }
            }
        }

        Destroy(warning);

        // 攻撃後の待機時間
        yield return new WaitForSeconds(jumpEndWaitTime);

        //Debug.Log("Jump attack ends / ジャンプ攻撃終了");
        Debug.Log(this.gameObject.name + " Jump attack ends / " + this.gameObject.name + " がジャンプ攻撃終了");
    }
}
