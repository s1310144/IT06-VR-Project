using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public float waitTime = 1.5f;
    public float jumpTime = 1.5f;
    public GameObject jumpAreaPrefab;

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
        Vector3 startPos = transform.position;

        // 危険エリア生成
        GameObject warning = Instantiate(jumpAreaPrefab, targetPos, Quaternion.identity);
        warning.transform.localScale = new Vector3(damageRange * 2, 1, damageRange * 2);

        // 予備動作
        yield return new WaitForSeconds(waitTime);

        float timer = 0f;

        while (timer < jumpTime)
        {
            timer += Time.deltaTime;

            float t = timer / jumpTime;

            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            pos.y += Mathf.Sin(t * Mathf.PI) * 5f;

            transform.position = pos;

            yield return null;
        }

        sound.PlayJump();

        // 着地ダメージ
        Collider[] hits =
            Physics.OverlapSphere(targetPos, damageRange);

        foreach (Collider hit in hits)
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
    }
}
