using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider energySlider;
    public Image energyFillImage;

    [Header("Energy Logic")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public bool canUseUltimate = false;

    // Tham chiếu đến bộ não
    private AttributesManager stats;

    void Awake()
    {
        stats = GetComponent<AttributesManager>();
    }

    void OnEnable()
    {
        // Đăng ký lắng nghe sự kiện máu thay đổi từ AttributesManager
        stats.OnHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        // Hủy đăng ký để tránh lỗi bộ nhớ
        stats.OnHealthChanged -= UpdateHealthUI;
    }

    void Start()
    {
        currentEnergy = 0;

        // Khởi tạo giá trị cho UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = stats.maxHealth;
            healthSlider.value = stats.maxHealth;
        }
        
        UpdateEnergyUI();
    }

    // TỐI ƯU: Xóa bỏ Update() liên tục, chỉ cập nhật khi cần
    void UpdateHealthUI(int currentHP, int maxHP)
    {
        if (healthSlider != null) 
        {
            healthSlider.maxValue = maxHP;
            healthSlider.value = currentHP;
        }
        if (currentHP <= 0) Debug.Log("Player Die!");
    }

    public void ChangeEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateEnergyUI();
    }

    void UpdateEnergyUI()
    {
        if (energySlider != null) energySlider.value = currentEnergy;

        if (currentEnergy >= maxEnergy)
        {
            canUseUltimate = true;
            if (energyFillImage != null) energyFillImage.color = Color.yellow;
        }
        else
        {
            canUseUltimate = false;
            if (energyFillImage != null) energyFillImage.color = Color.cyan;
        }
    }
}