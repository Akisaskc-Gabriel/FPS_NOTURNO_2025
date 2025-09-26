using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private Rigidbody rb;
    public float speed;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public ZombieState currentState = ZombieState.Idle;

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
        if (currentState == ZombieState.Dead) return;

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

        UpdateAnimator();

    }

    void SetState(ZombieState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    public void TakeDamage()
    {
        SetState(ZombieState.Damaged);
        UpdateAnimator();
    }

    public void Die()
    {
        SetState(ZombieState.Dead);
        UpdateAnimator();
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
        if (currentState == ZombieState.Walking)
            MoveZombie();
    }

    void MoveZombie()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPos = rb.position + direction * speed * Time.deltaTime;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        rb.MovePosition(newPos);
    }

}
