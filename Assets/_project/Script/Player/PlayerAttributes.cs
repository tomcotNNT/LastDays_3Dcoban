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
        // Cố gắng tìm AttributesManager
        stats = GetComponent<AttributesManager>();

        // Nếu vẫn không thấy, báo lỗi rõ ràng để bạn đi kiểm tra Inspector
        if (stats == null)
        {
            Debug.LogError("LỖI: Quên chưa gắn script AttributesManager vào Player rồi bạn ơi!");
        }
    }

    void OnEnable()
    {
        // Chỉ đăng ký nếu stats không bị null
        if (stats != null)
        {
            stats.OnHealthChanged += UpdateHealthUI;
        }
    }

    void OnDisable()
    {
        // Kiểm tra NULL trước khi hủy đăng ký (Đây là dòng 33 của bạn)
        if (stats != null)
        {
            stats.OnHealthChanged -= UpdateHealthUI;
        }
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