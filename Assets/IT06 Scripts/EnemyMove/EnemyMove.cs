using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 2f;
    public float[] randomMoveTimerRange = { 1.0f, 3.0f };

    public float cooldown = 2f;

    public float stopRange = 2.5f;
    public float searchRange = 10f;
    public float enemySizeRadius = 1.45f;

    public float floatBaseY = 0.2f;
    public float floatHeight = 0.1f;
    public float floatSwaySpeed = 5.0f;

    public float maxSlopeAngle = 45f;

    private bool playerInStage = false;

    public EnemySound _enemySound;

    // デバッグ用
    public bool activeMove = true;
    public bool activeRoar = true;
    public bool onlySpin = false;
    public bool onlyJump = false;
    public bool onlyBeam = false;
    public bool activeDebugCooldown = true;


    private Vector3 startPos;

    private int previousAttack = -1;

    private float cooldownTimer = 0f;

    bool foundPlayer = false;
    bool isRoar = false;

    bool isAttack = false;
    SpinAttack _spinAttack;
    JumpAttack _jumpAttack;
    BeamAttack _beamAttack;

    Roar _roar;

    EnemyHealth _enemyHealth;

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
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!activeMove) return;

        if (_enemyHealth.isDead) return;
        SearchPlayer();

        if (!isRoar && playerInStage)
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
        RaycastHit hit;
        Vector3 pos = transform.position;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f)) 
        {
            pos = transform.position;

            pos.y = hit.point.y + floatBaseY + Mathf.Sin(Time.time * floatSwaySpeed) * floatHeight;
        }

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
            Vector3 nextPos =　transform.position + dir * moveSpeed * Time.deltaTime;

            if (CanMove(nextPos))
            {
                transform.position = nextPos;
            }
        }

        // プレイヤーのほうを向く
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
        Vector3 dir;

        if (randomMoveTimer <= 0)
        {
            dir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            randomDir = dir.normalized;
            randomMoveTimer = Random.Range(randomMoveTimerRange[0], randomMoveTimerRange[1]);
        }

        Vector3 nextPos =　transform.position + randomDir * moveSpeed * Time.deltaTime;

        if (CanMove(nextPos))
        {
            transform.position = nextPos;
        }
        else 
        {
            dir = -randomDir;
            dir = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            randomDir = dir.normalized;
        }

            transform.rotation = Quaternion.LookRotation(randomDir);
    }

    bool CanMove(Vector3 nextPos)
    {
        RaycastHit hit;
        Vector3 moveDir = (nextPos - transform.position).normalized;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f + moveDir * enemySizeRadius;

        if (Physics.Raycast(rayOrigin, moveDir, out hit, 1.0f))
        {
            if(stopRange < Vector3.Distance(hit.transform.position, transform.position)) return false;
        }

        if (Physics.Raycast(nextPos + moveDir * enemySizeRadius + Vector3.up * 2f, Vector3.down, out hit, 10f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            return slopeAngle <= maxSlopeAngle;
        }

        return false;
    }

    void SelectAttack()
    {
        if (isAttack || cooldownTimer > 0 || isRoar) return;

        if(DebugAttack()) return;

        int attack;

        attack = Random.Range(0, 3);
        while (attack == previousAttack) 
        {
            attack = Random.Range(0, 3);
        }
        previousAttack = attack;

        switch (attack)
        {
            case 0:
                StartCoroutine(Spin());
                break;

            case 1:
                StartCoroutine(Jump());
                break;
            
            case 2:
                StartCoroutine(Beam());
                break;
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
        if(!activeRoar) yield break;

        isRoar = true;

        yield return StartCoroutine(_roar.RoarStart(player, _enemySound));

        foundPlayer = true;

        isRoar = false;
    }

    // デバッグ用変数が一つでもtrueの時falseを返す
    bool DebugAttack() 
    {
        if (onlySpin)
        {
            StartCoroutine(Spin());
            cooldownTimer = cooldown;
            return true;
        }
        else if (onlyJump)
        {
            StartCoroutine(Jump());
            cooldownTimer = cooldown;
            return true;
        }
        else if (onlyBeam)
        {
            StartCoroutine(Beam());
            cooldownTimer = cooldown;
            return true;
        }
        else {
            cooldownTimer = cooldown;
            return false;
        }
    }

    public void PlayerInStage() { playerInStage = true; }
}
