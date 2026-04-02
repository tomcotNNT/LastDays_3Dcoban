using UnityEngine;
using UnityEngine.UI; // Đảm bảo đã có dòng này

public class ZombieHealth : MonoBehaviour
{
    public Slider healthSlider; // Bây giờ chữ Slider sẽ hết bị gạch đỏ
    public float health;
    public bool isElite = false;

    void Start()
    {
        // Khởi tạo máu dựa trên loại Zombie
        health = isElite ? 600f : 400f;

        // Cập nhật giá trị cực đại cho thanh máu trên đầu Zombie
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}