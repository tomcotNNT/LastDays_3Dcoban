using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public PlayerAttributes playerAttr;

    [Header("Colliders (Kéo từ chân/búa vào đây)")]
    public Collider hammerCollider; // Collider gắn trên cái Búa
    public Collider footCollider;   // Collider gắn trên bàn chân (KickCollider)

    private int currentAttackType = 1; // 1: Đá, 2: Vung ngang, 3: Combo, 4: Ulti
    private bool hasHitInThisSwing = false; // Kiểm tra đã trúng địch trong lần vung này chưa

    void Start()
    {
        // Đảm bảo lúc đầu cả 2 collider đều tắt
        if (hammerCollider != null) hammerCollider.enabled = false;
        if (footCollider != null) footCollider.enabled = false;
    }

    // --- 1. GỌI TỪ ANIMATION EVENT (ĐẶT Ở FRAME ĐẦU TIÊN) ---
    public void SetAttackType(int type)
    {
        currentAttackType = type;
        hasHitInThisSwing = false; // Reset trạng thái cho đòn đánh mới
        Debug.Log($"<color=cyan>Bắt đầu chuẩn bị Attack Type: {type}</color>");
    }

    // --- 2. GỌI TỪ ANIMATION EVENT (BẬT/TẮT VA CHẠM) ---
    public void EnableWeaponCollider(int isEnable)
    {
        bool isActive = (isEnable == 1);

        // Logic thông minh: Đòn 1 dùng chân, các đòn khác dùng búa
        if (currentAttackType == 1)
        {
            if (footCollider != null) footCollider.enabled = isActive;
            if (hammerCollider != null) hammerCollider.enabled = false;
        }
        else
        {
            if (hammerCollider != null) hammerCollider.enabled = isActive;
            if (footCollider != null) footCollider.enabled = false;
        }
    }

    // --- 3. GỌI TỪ ANIMATION EVENT (KẾT THÚC VUNG) ---
    public void OnAttackEnd()
    {
        // Tắt tất cả collider để an toàn
        if (hammerCollider != null) hammerCollider.enabled = false;
        if (footCollider != null) footCollider.enabled = false;

        // Nếu kết thúc cả quá trình vung mà không trúng ai thì mới trừ nội năng
        if (!hasHitInThisSwing)
        {
            playerAttr.ChangeEnergy(-5f);
            Debug.Log("<color=red>Đánh hụt hoàn toàn! -5 Nội năng</color>");
        }
    }

    // Xử lý khi Collider chạm vào Zombie
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Tránh việc 1 lần vung búa chạm 1 mục tiêu nhiều lần (trừ Attack 3 là combo nhiều nhịp)
            if (hasHitInThisSwing && currentAttackType != 3) return;

            float damage = CalculateDamage();
            var zombieHealth = other.GetComponent<ZombieHealth>();

            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
                
                // Đánh trúng: +20 Nội năng
                playerAttr.ChangeEnergy(20f);
                hasHitInThisSwing = true; 

                Debug.Log($"<color=green>Trúng đích!</color> Gây {damage} sát thương lên {other.name}");
            }
        }
    }

    // Hàm tính toán sát thương theo yêu cầu của bạn
    float CalculateDamage()
    {
        switch (currentAttackType)
        {
            case 1: // Attack 1
                return Random.Range(10f, 30f);
            case 2: // Attack 2
                return 50f;
            case 3: // Attack 3
                return 50f;
            case 4: // Attack Until (Ultimate)
                // Dùng xong nội năng về 0
                playerAttr.currentEnergy = 0;
                playerAttr.canUseUltimate = false;
                
                // Tỉ lệ chí mạng 20% gây 300 dmg, còn lại 100 dmg
                if (Random.value < 0.2f) return 300f;
                return 100f;
            default:
                return 0;
        }
    }
}