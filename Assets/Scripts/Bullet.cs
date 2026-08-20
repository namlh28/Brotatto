using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;   // Tốc độ bay
    [SerializeField] private float lifeTime = 3f; // Tự xóa sau 3s nếu không trúng gì

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifeTime); // Đảm bảo đạn không tồn tại mãi mãi
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Bỏ qua nếu chạm phải Player hoặc bất kỳ đồ vật con nào thuộc Player (Súng)
        if (collision.CompareTag("Player") || collision.transform.parent == transform.parent) 
        {
            return;
        }

        // 2. Nếu trúng Quái (Duy nhất cần check Tag này)
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f); // Gây 10 dame
            }
            Destroy(gameObject); // Nổ đạn ngay
        }
        // 3. Nếu chạm bất kỳ vật thể có Collider nào khác (Tường, Map, Vật cản...) -> Nổ đạn luôn
        else
        {
            Destroy(gameObject);
        }
    }
}