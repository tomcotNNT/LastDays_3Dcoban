using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider energySlider;
    public Image energyFillImage; // Kéo đối tượng 'Fill' của EnergyBar vào đây

    [Header("Health & Energy")]
    public float maxHealth = 1000f;
    public float currentHealth;
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public bool canUseUltimate = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;

        // Thiết lập giá trị ban đầu cho UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
            if (energyFillImage != null) energyFillImage.color = Color.cyan;
        }
    }

    void Update()
    {
        // Cập nhật thanh UI liên tục
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (energySlider != null) energySlider.value = currentEnergy;
    }

    public void ChangeEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        // Logic xử lý khi đầy nội năng
        if (currentEnergy >= maxEnergy)
        {
            canUseUltimate = true;
            if (energyFillImage != null) energyFillImage.color = Color.yellow; 
            Debug.Log("<color=yellow>ULTIMATE READY!</color>");
        }
        else
        {
            canUseUltimate = false;
            if (energyFillImage != null) energyFillImage.color = Color.cyan;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0) Debug.Log("Player Die!");
    }
}