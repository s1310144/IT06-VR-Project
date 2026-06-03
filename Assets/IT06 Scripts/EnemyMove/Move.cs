using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 2f;

    public float cooldown = 2f;

    public float stopRange = 2.5f;
    public float searchRange = 10f;

    public float floatBaseY = 0.2f;
    public float floatHeight = 0.1f;
    public float floatSwaySpeed = 5.0f;

    public EnemySound _enemySound;

    private Vector3 startPos;

    private int at = 0;

    private float cooldownTimer = 0f;

    bool foundPlayer = false;
    bool isRoar = false;

    bool isAttack = false;
    SpinAttack _spinAttack;
    JumpAttack _jumpAttack;
    BeamAttack _beamAttack;

    Roar _roar;

    Vector3 randomDir;

    float randomMoveTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        _spinAttack = GetComponent<SpinAttack>();
        _jumpAttack = GetComponent<JumpAttack>();
        _beamAttack = GetComponent<BeamAttack>();
        _roar = GetComponent<Roar>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SearchPlayer();

        if (!isRoar)
        {
            if (foundPlayer)
            {
                AttackMove();

                SelectAttack();
            }
            else
            {
                RandomMove();
            }
        }

        FloatEnemy();
    }

    void SearchPlayer()
    {
        if (foundPlayer) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= searchRange)
        {
            foundPlayer = true;
            StartCoroutine(Roar());
        }
    }

    void FloatEnemy()
    {
        Vector3 pos = transform.position;

        pos.y = floatBaseY + startPos.y + Mathf.Sin(Time.time * floatSwaySpeed) * floatHeight;

        transform.position = pos;
    }

    void AttackMove()
    {
        if (isAttack) return;

        cooldownTimer -= Time.deltaTime;

        Vector3 enemyPos = transform.position;
        Vector3 playerPos = player.position;

        enemyPos.y = 0;
        playerPos.y = 0;

        Vector3 dir = (playerPos - enemyPos).normalized;
        float distance = Vector3.Distance(enemyPos, playerPos);

        if (distance > stopRange)
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
        }

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }
    }

    void RandomMove()
    {
        if (isAttack) return;

        randomMoveTimer -= Time.deltaTime;

        if (randomMoveTimer <= 0)
        {
            Vector3 dir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            //dir.y = 0;
            randomDir = dir.normalized;
            randomMoveTimer = Random.Range(2f, 4f);
        }

        transform.position +=
            randomDir * moveSpeed * Time.deltaTime;

        transform.rotation =
            Quaternion.LookRotation(randomDir);
    }

    void SelectAttack()
    {
        if (isAttack || cooldownTimer > 0 || isRoar) return;

        at++;
        //at = 2;
        if (at >= 3) at = 0;

        if (at == 0)
        {
            StartCoroutine(Spin());
        }
        else if (at == 1)
        {
            StartCoroutine(Jump());
        }
        else if (at == 2)
        {
            StartCoroutine(Beam());
        }

        cooldownTimer = cooldown;
    }

    IEnumerator Spin()
    {
        isAttack = true;

        yield return StartCoroutine(_spinAttack.Attack(_enemySound));

        isAttack = false;
    }

    IEnumerator Jump()
    {
        isAttack = true;

        yield return StartCoroutine(_jumpAttack.Attack(player.position, _enemySound));

        isAttack = false;
    }

    IEnumerator Beam()
    {
        isAttack = true;

        yield return StartCoroutine(_beamAttack.Attack(player, _enemySound));

        isAttack = false;
    }

    IEnumerator Roar()
    {
        isRoar = true;

        yield return StartCoroutine(_roar.RoarStart(player, _enemySound));

        foundPlayer = true;

        isRoar = false;
    }
}
