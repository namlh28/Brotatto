using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("UI & References")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    private float currentSpeed;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // Internal Components
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false;
    private Vector2 movement;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        if (isDead)
        {
            movement = Vector2.zero;
            UpdateAnimation(0);
            return;
        }

        // 1. Lấy Input di chuyển
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        // 2. Xử lý tốc độ Chạy (Shift) / Đi bộ
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 3. Cập nhật Animation
        UpdateAnimation(movement.magnitude);

        // 4. Lật hướng nhân vật trái/phải
        HandleFlip();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            rb.linearVelocity = movement * currentSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleFlip()
    {
        if (movement.x != 0)
        {
            // Nếu di chuyển sang trái (movement.x < 0) thì flipX = true, ngược lại false
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    private void UpdateAnimation(float speed)
    {
        if (anim != null)
        {
            // An toàn kiểm tra trước khi gán để tránh báo lỗi Console
            anim.SetFloat("speed", speed);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.SetTrigger("die");
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
        }
    }
}