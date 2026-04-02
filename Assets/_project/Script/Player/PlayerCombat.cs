using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerAttributes playerAttr;
    private int currentAttackType = 1; // 1: A1, 2: A2, 3: A3, 4: Ulti

    // Gọi hàm này từ Animation Event (giống như bạn đã làm ở các bước trước)
    public void SetAttackType(int type)
    {
        currentAttackType = type;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đánh trúng Zombie (Layer hoặc Tag là Enemy)
        if (other.CompareTag("Enemy"))
        {
            float damage = CalculateDamage();
            
            // Gửi sát thương tới Zombie
            var zombieHealth = other.GetComponent<ZombieHealth>(); 
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
                
                // Đánh trúng: Tăng 20 nội năng
                playerAttr.ChangeEnergy(20f);
                Debug.Log($"Trúng đích! Gây {damage} HP. +20 Nội năng");
            }
        }
    }

    // Hàm tính toán sát thương theo quy tắc của bạn
    float CalculateDamage()
    {
        switch (currentAttackType)
        {
            case 1: // Attack 1
                return Random.Range(10, 31);
            case 2: // Attack 2
                return 50f;
            case 3: // Attack 3
                return 50f;
            case 4: // Attack Until (Ultimate)
                playerAttr.currentEnergy = 0; // Dùng xong về 0
                playerAttr.canUseUltimate = false;
                
                // Tỉ lệ chí mạng 300 dmg (ví dụ 20% cơ hội)
                if (Random.value < 0.2f) return 300f; 
                return 100f;
            default:
                return 0;
        }
    }

    // Hàm này gọi khi kết thúc Animation mà không chạm vào quái (Hụt)
    public void OnAttackMiss()
    {
        playerAttr.ChangeEnergy(-5f);
        Debug.Log("Đánh hụt! -5 Nội năng");
    }
}