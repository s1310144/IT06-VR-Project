using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    public Transform beamStart;

    public GameObject beamObject;

    public GameObject beamBulletPrefab;

    public float waitTime = 2.0f;

    public float beamWidth = 0.3f;

    public float beamRange = 20f;

    public float beamLimitAngle = 80f;

    public int damage = 5;

    public float ballSpeed = 15f;

    public float beamLengthOffset = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        beamObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Attack(Transform player, EnemySound sound)
    {
        beamObject.SetActive(true);

        Vector3 targetPos = player.position + Vector3.up * 1.0f;

        float timer = 0f;


        while (timer < waitTime)
        {
            timer += Time.deltaTime;

            targetPos = player.position + Vector3.up * 1.0f;

            //角度制限
            Vector3 dir = (targetPos - beamStart.position).normalized;

            Vector3 forward = beamStart.forward;

            float angle = Vector3.SignedAngle(forward, dir, Vector3.up);

            float clampedAngle = Mathf.Clamp(angle, -beamLimitAngle, beamLimitAngle);

            Vector3 clampedDir = Quaternion.AngleAxis(clampedAngle, Vector3.up) * forward;

            //レイキャスト
            RaycastHit hit;

            float distance;
            float playerDistance = Vector3.Distance(beamStart.position, targetPos);

            // Raycast
            if (Physics.Raycast(beamStart.position, dir, out hit, playerDistance))
            {
                distance = hit.distance;
                targetPos = hit.point;
            }
            else
            {
                distance = playerDistance;
                targetPos = beamStart.position + dir * beamRange;
            }

            // 中間位置
            beamObject.transform.position = beamStart.position + dir * distance * 0.5f;

            // 向き
            beamObject.transform.rotation = Quaternion.LookRotation(dir);

            distance += distance * beamLengthOffset;

            // サイズ
            beamObject.transform.localScale = new Vector3(beamWidth, beamWidth, distance);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        beamObject.SetActive(false);

        GameObject bullet = Instantiate(beamBulletPrefab, beamStart.position, Quaternion.identity);

        BeamBullet _beamBullet = bullet.GetComponent<BeamBullet>();
        _beamBullet.damage = this.damage;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Vector3 bulletDir = (targetPos - beamStart.position).normalized;

        rb.velocity = bulletDir * ballSpeed;

        sound.PlayBeam();

        Debug.Log("ビーム終了");
    }
}
