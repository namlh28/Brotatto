using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float health = 10f;

    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Lấy component Animator
    }

    private void Update()
    {
        if (isDead) return; // Nếu đã chết thì ngừng di chuyển
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (playerTransform == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position, 
            playerTransform.position, 
            moveSpeed * Time.deltaTime
        );

        spriteRenderer.flipX = (playerTransform.position.x < transform.position.x);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        
        // Tắt Collider để đạn không bắn trúng quái đã chết nữa
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Kích hoạt Animation Die
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }

        // Chờ 0.5 giây cho chạy xong Animation rồi mới xóa GameObject
        Destroy(gameObject, 0.5f);
    }
}