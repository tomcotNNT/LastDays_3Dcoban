using UnityEngine;
using UnityEngine.UI;
using System;

public class AttributesManager : MonoBehaviour
{
    [Header("UI & Setup")]
    public Slider healthSlider; // Dành cho Zombie (Player dùng script riêng nên có thể để trống ô này trên Player)
    public bool isElite = false;

    [Header("Chỉ số cơ bản")]
    public string characterName;
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    // Thuộc tính để PlayerAttributes hoặc SimpleZombieAI truy cập
    public int Health => currentHealth;

    [Header("Chiến đấu")]
    public int attack = 10;
    [Range(0, 90)] public int armor = 10;
    public float critDamage = 1.5f;
    [Range(0f, 1f)] public float critChance = 0.2f;

    // SỰ KIỆN QUAN TRỌNG: Để PlayerAttributes lắng nghe và cập nhật thanh máu
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        // Nếu là Zombie thì mới tự set máu theo loại, nếu là Player thì giữ nguyên maxHealth bạn chỉnh
        if (gameObject.tag != "Player")
        {
            maxHealth = isElite ? 600 : 400;
        }
        
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Thông báo cho UI lần đầu tiên
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        float reduction = amount * (armor / 100f);
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(amount - reduction));

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Kích hoạt sự kiện để PlayerAttributes.cs tự động nhảy thanh máu
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Nếu là Zombie thì tự cập nhật Slider trên đầu nó
        if (healthSlider != null) healthSlider.value = currentHealth;

        Debug.Log($"<color=red>{characterName}</color> nhận {finalDamage} sát thương. Còn {currentHealth} HP");

        if (currentHealth <= 0) Die();
    }

    public void DealDamage(GameObject target)
    {
        if (target == null) return;
        AttributesManager targetStats = target.GetComponent<AttributesManager>();
        
        if (targetStats != null)
        {
            float totalDamage = attack;
            if (UnityEngine.Random.value < critChance) totalDamage *= critDamage;
            targetStats.TakeDamage(Mathf.RoundToInt(totalDamage));
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null) anim.SetTrigger("Death");

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        Debug.Log($"{characterName} đã gục ngã!");
        Destroy(gameObject, 3f);
    }
}