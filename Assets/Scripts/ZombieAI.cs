using System;
using UnityEngine;

public class ZombieAI : CharacterBase
{
    private Animator animator;
    private Transform player;
    private Rigidbody rb;
    public float speed;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public ZombieState currentState = ZombieState.Idle;

    float canMoveCounter = 0f;
    bool canMove = true;
    public float zombieDamage = 10f;
    bool isDead = false;

    public GameObject attackCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            currentState = ZombieState.Dead;
        }
        if (currentState == ZombieState.Dead) return;

        CanMoveCounter();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackRange)
        {
            SetState(ZombieState.Attacking);
        }
        else if (distance < detectionRange)
        {
            SetState(ZombieState.Walking);
        }
        else
        {
            SetState(ZombieState.Idle);
        }

        if (currentState == ZombieState.Attacking)
        {
            canMove = false;
        }

        UpdateAnimator();

    }

    void SetState(ZombieState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (currentState == ZombieState.Dead) return;
        base.TakeDamage(damage);
        SetState(ZombieState.Damaged);
        canMove = false;
        UpdateAnimator();
    }

    protected override void Die()
    {
        isDead = true;
        SetState(ZombieState.Dead);
        UpdateAnimator();
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        if (rb != null) rb.isKinematic = true;
    }

    void UpdateAnimator()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetBool("walk", false);
                break;
            case ZombieState.Walking:
                animator.SetBool("walk", true);
                break;
            case ZombieState.Attacking:
                animator.SetTrigger("atk");
                animator.SetBool("walk", false);
                break;
            case ZombieState.Damaged:
                animator.SetBool("walk", false);
                animator.SetTrigger("dmg");
                break;
            case ZombieState.Dead:
                animator.SetBool("walk", false);
                animator.SetBool("die", true);
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == ZombieState.Walking && canMove)
            MoveZombie();
    }

    void MoveZombie()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPos = rb.position + direction * speed * Time.deltaTime;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        rb.MovePosition(newPos);
    }

    void CanMoveCounter()
    {
        if (canMove == false)
        {
            canMoveCounter += Time.deltaTime;
            if (canMoveCounter >= 1.8f)
            {
                canMove = true;
                canMoveCounter = 0f;
            }
        }
    }

    public void EnableAttack()
    {
        attackCollider.SetActive(true);
    }

    public void DisableAttack()
    {
        attackCollider.SetActive(false);
    }

    private void OnTriggerEnter(Collider Others)
    {
        if (Others.CompareTag("Player"))
        { 
            PlayerController player = Others.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(zombieDamage);
            }
        }
    }

}
