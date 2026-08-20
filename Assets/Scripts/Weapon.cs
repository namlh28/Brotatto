using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float offsetDistance = 0.5f; // Khoảng cách súng quanh Player
    [SerializeField] private float rotationSpeed = 25f;  // Độ mượt xoay súng

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab; // Prefab đạn
    [SerializeField] private Transform firePoint;     // Vị trí nòng súng
    [SerializeField] private float fireRate = 0.15f;   // Tốc độ bắn (s/viên)

    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private float nextFireTime = 0f;
    private Vector2 currentAimDirection = Vector2.right; // Hướng mặc định

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            playerTransform = transform.parent;
        }
    }

    private void Update()
    {
        // Hiện tại test trên PC bằng phím mũi tên / WASD để giả lập hướng ngắm
        UpdateAimDirection();
        RotateWeapon();
        HandleAutoShoot();
    }

    // Hàm cập nhật hướng ngắm (Sau này chỉ cần thay bằng Input từ Virtual Joystick)
    private void UpdateAimDirection()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Nếu người chơi đang di chuyển, cập nhật hướng súng theo hướng di chuyển
        if (moveX != 0 || moveY != 0)
        {
            currentAimDirection = new Vector2(moveX, moveY).normalized;
        }
    }

    private void RotateWeapon()
    {
        if (playerTransform == null) return;

        // 1. Đặt vị trí súng xoay quanh Player theo hướng ngắm
        Vector3 targetPos = playerTransform.position + (Vector3)currentAimDirection * offsetDistance;
        transform.position = targetPos;

        // 2. Tính góc xoay
        float angle = Mathf.Atan2(currentAimDirection.y, currentAimDirection.x) * Mathf.Rad2Deg;

        // 3. Xoay súng mượt mà
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 4. Lật súng khi quay sang trái để không bị ngược bụng
        spriteRenderer.flipY = (angle > 90 || angle < -90);
    }

    private void HandleAutoShoot()
    {
        // Tự động bắn liên tục theo thời gian (Chuẩn phong cách Brotato / Survivor Mobile)
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Tạo ra viên đạn tại nòng súng
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}