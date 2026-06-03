using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Roar : MonoBehaviour
{
    public float lookPlayerTime = 1f;

    public float roarUpTime = 0.5f;

    public float roarWaitTime = 1f;

    public float roarAngle = -30f;

    EnemySound _enemySound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RoarStart(Transform player, EnemySound sound)
    {
        yield return new WaitForSeconds(0.2f);

        // プレイヤー方向
        Vector3 dir =
            (player.position - transform.position).normalized;

        dir.y = 0;

        Quaternion startRot =
            transform.rotation;

        Quaternion lookRot =
            Quaternion.LookRotation(dir);

        float timer = 0f;

        // プレイヤーを見る
        while (timer < lookPlayerTime)
        {
            timer += Time.deltaTime;

            transform.rotation =
                Quaternion.Slerp(
                    startRot,
                    lookRot,
                    timer / lookPlayerTime
                );

            yield return null;
        }

        transform.rotation = lookRot;

        // 上向き
        Quaternion roarRot =
            lookRot *
            Quaternion.Euler(roarAngle, 0, 0);

        yield return new WaitForSeconds(0.5f);


        timer = 0f;

        sound.PlayRoar();

        // 顔を上げる
        while (timer < roarUpTime)
        {
            timer += Time.deltaTime;

            transform.rotation =
                Quaternion.Slerp(
                    lookRot,
                    roarRot,
                    timer / roarUpTime
                );

            yield return null;
        }

        // 咆哮
        yield return new WaitForSeconds(roarWaitTime);

        timer = 0f;

        // 戻す
        while (timer < roarUpTime)
        {
            timer += Time.deltaTime;

            transform.rotation =
                Quaternion.Slerp(
                    roarRot,
                    lookRot,
                    timer / roarUpTime
                );

            yield return null;
        }

        transform.rotation = lookRot;

        yield return new WaitForSeconds(1f);
    }
}
