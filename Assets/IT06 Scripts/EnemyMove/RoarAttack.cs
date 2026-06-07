using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarAttack : MonoBehaviour
{
    public float lookPlayerTime = 1f;
    public float FaceUpWaitTime = 0.5f;
    public float roarFaceUpTime = 0.75f;
    public float roarTime = 1f;
    public float roarFaceDownTime = 0.25f;
    public float roarEndWaitTime = 1.0f;

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

    public IEnumerator Roar(Transform player, EnemySound sound)
    {
        Debug.Log("Roar attack begins / 咆哮攻撃開始");
        // プレイヤー方向などを計算
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        Quaternion startRot = transform.rotation;
        Quaternion lookRot = Quaternion.LookRotation(dir);

        float timer = 0f;

        // プレイヤーを見る
        while (timer < lookPlayerTime)
        {
            timer += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(startRot, lookRot, timer / lookPlayerTime);

            yield return null;
        }

        yield return new WaitForSeconds(FaceUpWaitTime);


        transform.rotation = lookRot;

        // 上向き
        Quaternion roarRot = lookRot * Quaternion.Euler(roarAngle, 0, 0);


        timer = 0f;

        // 咆哮音開始
        sound.PlayRoar();

        // 顔を上げる
        while (timer < roarFaceDownTime)
        {
            timer += Time.deltaTime;

            transform.rotation =Quaternion.Slerp(lookRot, roarRot, timer / roarFaceDownTime);

            yield return null;
        }

        yield return new WaitForSeconds(roarTime);

        timer = 0f;

        // 戻す
        while (timer < roarFaceUpTime)
        {
            timer += Time.deltaTime;

            transform.rotation = Quaternion.Slerp( roarRot, lookRot, timer / roarFaceUpTime);

            yield return null;
        }

        transform.rotation = lookRot;

        yield return new WaitForSeconds(roarEndWaitTime);

        Debug.Log("Roar attack ends / 咆哮攻撃終了");
    }
}
