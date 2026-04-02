using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    [Header("Chỉ số cơ bản")]
    public string characterName;
    public int health = 100;
    public int attack = 10;

    [Header("Chỉ số chí mạng")]
    [Tooltip("1.5 tương đương damage tăng 150%")]
    public float critDamage = 1.5f;
    
    [Range(0f, 1f)]
    [Tooltip("0.5 tương đương 50% tỉ lệ ra đòn chí mạng")]
    public float critChance = 0.2f; // Mặc định 20% cho cân bằng

    // ====== NHẬN SÁT THƯƠNG ======
    public void TakeDamage(int amount)
    {
        health -= amount;

        // Đảm bảo máu không xuống dưới 0 để log nhìn đẹp hơn
        health = Mathf.Max(0, health);

        Debug.Log($"<color=red>{characterName}</color> nhận {amount} damage. Máu còn lại: {health}");

        if (health <= 0)
        {
            Debug.Log($"<b>{characterName}</b> is Dead!");
        }
    }

    // ====== GÂY SÁT THƯƠNG ======
    public void DealDamage(GameObject target)
    {
        AttributesManager atm = target.GetComponent<AttributesManager>();
        if (atm == null) return;

        float totalDamage = attack;
        bool isCrit = false;

        // Tính toán tỉ lệ chí mạng
        if (Random.Range(0f, 1f) < critChance)
        {
            totalDamage *= critDamage;
            isCrit = true;
        }

        // Chuyển đổi sang kiểu int để trừ máu
        int finalDamage = Mathf.RoundToInt(totalDamage);

        // Log thông báo cụ thể
        if (isCrit)
        {
            Debug.Log($"<color=yellow>CRITICAL HIT!</color> {characterName} đánh cực mạnh gây {finalDamage} sát thương!");
        }
        else
        {
            Debug.Log($"{characterName} gây {finalDamage} sát thương cơ bản.");
        }

        atm.TakeDamage(finalDamage);
    }
    
}