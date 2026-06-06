using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    public Transform[] laserStart = new Transform[2];
    public GameObject laserObjectPrefab;
    public GameObject bulletPrefab;


    public float laserTime = 2.0f;
    public float bulletWaitTime = 0.75f;
    public float beamEndWaitTime = 1.0f;
    //public float conAttacksWaitTime = 0.1f;
    public int consecutiveAttacksNumber = 2;
    public float consecutiveAttacksTimeRatio = 0.1f;


    public float laserWidth = 0.02f;
    public float laserRange = 100f;

    public float laserLimitAngle = 80f;


    public int damage = 5;

    public float bulletSpeed = 15f;

    //public float beamLengthOffset = 0.05f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Attack(Transform player, EnemySound sound)
    {
        yield return LaserAndBulletAttack(player, sound, 1.0f);

        for (int i = 0; i < consecutiveAttacksNumber; i++) 
        {
            yield return LaserAndBulletAttack(player, sound, consecutiveAttacksTimeRatio);
        }

        // 攻撃後の待機時間
        yield return new WaitForSeconds(beamEndWaitTime);
    }

    IEnumerator LaserAndBulletAttack(Transform player, EnemySound sound, float waitTimeRatio)
    {
        Vector3 targetPos = player.position + Vector3.up * 1.0f;

        GameObject[] lasers = new GameObject[laserStart.Length];
        lasers[0] = Instantiate(laserObjectPrefab);
        lasers[1] = Instantiate(laserObjectPrefab);
        //for (int i = 0; i < laserStart.Length; i++) { }

        Vector3[] clampedDir =new Vector3[laserStart.Length]; 

        float timer = 0f;

        // プレイヤーを追いかけるレーザーポインターwp出す
        while (timer < laserTime * waitTimeRatio)
        {
            timer += Time.deltaTime;

            targetPos = player.position + Vector3.up * 1.0f;

            UseLaserPointer(laserStart[0], targetPos, lasers[0]);
            UseLaserPointer(laserStart[1], targetPos, lasers[1]);

            yield return null;
        }

        yield return null;
        //yield return new WaitForSeconds(bulletWaitTime * waitTimeRatio);

        timer = 0f;

        while (timer < bulletWaitTime * waitTimeRatio)
        {
            timer += Time.deltaTime;

            clampedDir[0] = UseLaserPointer(laserStart[0], targetPos, lasers[0]);
            clampedDir[1] = UseLaserPointer(laserStart[1], targetPos, lasers[1]);

            yield return null;
        }

        Destroy(lasers[0]);
        Destroy(lasers[1]);

        FireBullet(laserStart[0], targetPos, clampedDir[0], sound);
        FireBullet(laserStart[1], targetPos, clampedDir[1], sound);

        //yield return new WaitForSeconds(conAttacksWaitTime);
    }

    // レーザーポインター照射用
    Vector3 UseLaserPointer(Transform start, Vector3 targetPos, GameObject laser) 
    {
        //レーザーの角度制限を計算
        Vector3 dir = (targetPos - start.position).normalized;
        Vector3 forward = start.forward;
        float maxRadians = laserLimitAngle * Mathf.Deg2Rad;
        Vector3 clampedDir = Vector3.RotateTowards(start.forward, dir, maxRadians, 0f);



        RaycastHit hit;
        float distance;
        float playerDistance = Vector3.Distance(start.position, targetPos);

        // Raycast
        if (Physics.Raycast(start.position, clampedDir, out hit, playerDistance))
        {
            distance = hit.distance;
            targetPos = hit.point;
            Debug.Log(hit.collider.name);
        }
        else
        {
            distance = laserRange;
            targetPos = start.position + clampedDir * laserRange;
        }


        // Cubeを使うので中間位置を求める
        laser.transform.position = start.position + clampedDir * distance * 0.5f;

        // 向き
        laser.transform.rotation = Quaternion.LookRotation(clampedDir);

        //distance += distance * beamLengthOffset;

        // れあーざーに使うCubeの大きさ
        laser.transform.localScale = new Vector3(laserWidth, laserWidth, distance);

        //yield return null;

        return clampedDir;
    }

    void FireBullet(Transform start, Vector3 targetPos, Vector3 bulletDir, EnemySound sound) 
    {
        GameObject bullet = Instantiate(bulletPrefab, start.position, Quaternion.identity);

        BeamBullet _beamBullet = bullet.GetComponent<BeamBullet>();
        _beamBullet.damage = this.damage;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        bulletDir = (targetPos - start.position).normalized;

        rb.velocity = bulletDir * bulletSpeed;

        sound.PlayBeam();
    }
}
