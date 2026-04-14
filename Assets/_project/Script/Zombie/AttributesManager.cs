using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI; // Thêm để dùng NavMeshAgent trực tiếp

public class AttributesManager : MonoBehaviour
{
    [Header("UI & Setup")]
    public Slider healthSlider;
    public bool isElite = false;

    [Header("Chỉ số cơ bản")]
    public string characterName;
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public int Health => currentHealth;

    [Header("Chiến đấu")]
    public int attack = 10;
    [Range(0, 90)] public int armor = 10;
    public float critDamage = 1.5f;
    [Range(0f, 1f)] public float critChance = 0.2f;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        // Thiết lập máu dựa trên loại Enemy
        if (!CompareTag("Player")) // Dùng CompareTag sẽ hiệu quả hơn kiểm tra chuỗi trực tiếp
        {
            maxHealth = isElite ? 600 : 400;
        }
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        // Tính toán giáp
        float reduction = amount * (armor / 100f);
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(amount - reduction));

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI(); // Gọi hàm dùng chung để cập nhật cả Event và Slider

        Debug.Log($"<color=red>{characterName}</color> nhận {finalDamage} HP sát thương. Còn {currentHealth} HP");

        if (currentHealth <= 0) Die();
    }

    public void DealDamage(GameObject target)
    {
        if (target == null) return;
        var targetStats = target.GetComponent<AttributesManager>();

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

        // Ưu tiên tìm Animator ở chính nó trước, sau đó mới tìm ở con
        Animator anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        
        if (anim != null) anim.SetTrigger("Death");

        // Dừng di chuyển
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agent.isStopped = true;
            agent.enabled = false; // Tắt luôn agent để các zombie khác không bị vướng
        }

        Debug.Log($"{characterName} đã gục ngã!");
        
        // Tắt các va chạm để xác không cản đường người chơi
        if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        Destroy(gameObject, 3f);
    }
}