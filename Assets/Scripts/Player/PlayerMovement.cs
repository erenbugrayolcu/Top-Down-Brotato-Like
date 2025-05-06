using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    private float lastAttackTime;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;

    private Vector2 attackBoxSize = new Vector2(1.5f, 1f);

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        _rb.linearVelocity = _movement * _moveSpeed;

        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }

        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        _rb.linearVelocity = _movement * _moveSpeed;

        // animasyonlar vs. aynı kalabilir

        // saldırı
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime)
        {
            Attack();
            lastAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(int damage)
{
    currentHealth -= damage;
    currentHealth = Mathf.Max(currentHealth, 0);
    Debug.Log("Player has " + currentHealth + " health left.");
    if (currentHealth < 0)
    {
        Die();
    }
}

    private void Die()
    {
        Debug.Log("Player öldü");
        //death animation
    }

    private void Attack()
    {
        float lastHorizontal = _animator.GetFloat(_lastHorizontal);
        float lastVertical = _animator.GetFloat(_lastVertical);

        Vector2 attackDirection = new Vector2(lastHorizontal, lastVertical).normalized;

        // eğer hem yatay hem dikey sıfırsa varsayılan sağ yönü ver
        if (attackDirection == Vector2.zero)
            attackDirection = Vector2.right;

        Vector2 attackPosition = (Vector2)transform.position + attackDirection * 0.75f;

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackBoxSize, 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }

        Debug.Log("Vuruş yapıldı " + hitEnemies.Length + " düşmana");
    }
    private void OnDrawGizmosSelected()
    {
        if (_animator == null) return;

        float lastHorizontal = _animator.GetFloat(_lastHorizontal);
        float lastVertical = _animator.GetFloat(_lastVertical);
        Vector2 attackDirection = new Vector2(lastHorizontal, lastVertical).normalized;

        if (attackDirection == Vector2.zero)
            attackDirection = Vector2.right;

        Vector2 attackPosition = (Vector2)transform.position + attackDirection * 0.75f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPosition, attackBoxSize);
    }
}
