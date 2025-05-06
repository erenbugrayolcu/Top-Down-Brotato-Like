using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected Transform target;
    protected NavMeshAgent agent;

    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime;
    [SerializeField] private float attackRange = 1f;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    protected virtual void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);

    if (target != null)
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= attackRange && Time.time >= lastAttackTime)
        {
            Attack();
            lastAttackTime = Time.time + attackCooldown;
        }
    }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Attack()
{
    Debug.Log("Enemy saldırdı");
    PlayerMovement player = target.GetComponent<PlayerMovement>();
    if (player != null)
    {
        player.TakeDamage(attackDamage);
    }
}
    
}
