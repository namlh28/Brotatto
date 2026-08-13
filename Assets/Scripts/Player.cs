using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText; // Đã sửa tên biến
    [SerializeField] float moveSpeed = 5f;
    
    Animator anim;
    Rigidbody2D rb;

    
    int maxHealth;
    int currentHealth;

    bool dead = false; // Đã xóa dòng float dead thừa phía dưới

    float moveHorizontal, moveVertical;
    Vector2 movement;

    int facingDirection = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Đã sửa từ Animator sang Rigidbody2D
    }

    void Update()
    {
        if (dead)
        {
            movement = Vector2.zero;
            if (anim != null) anim.SetFloat("velocity", 0);
            return;
        }

        // Lấy Input di chuyển
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        // Cập nhật Animation (Đã sửa từ move -> movement)
        if (anim != null)
        {
            anim.SetFloat("velocity", movement.magnitude);
        }

        // Lật mặt nhân vật (Flip)
        if (movement.x != 0)
        {
            facingDirection = movement.x > 0 ? 1 : -1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    private void FixedUpdate()
    {
        // Áp dụng di chuyển vào Rigidbody2D (Đã sửa cú pháp nhân vector)
        if (!dead)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}