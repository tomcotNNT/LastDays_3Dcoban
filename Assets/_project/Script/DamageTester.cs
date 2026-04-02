using UnityEngine;

public class DamageTester : MonoBehaviour
{
    [Header("Cài đặt nhân vật")]
    public AttributesManager player;
    public AttributesManager enemy;

    private void Update()
    {
        // Khi nhấn phím O: Player tấn công Enemy
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (player != null && enemy != null)
            {
                player.DealDamage(enemy.gameObject);
            }
            else
            {
                Debug.LogWarning("Vui lòng kéo đủ Player và Enemy vào DamageTester!");
            }
        }

        // Khi nhấn phím P: Enemy tấn công Player
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (enemy != null && player != null)
            {
                enemy.DealDamage(player.gameObject);
            }
            else
            {
                Debug.LogWarning("Vui lòng kéo đủ Player và Enemy vào DamageTester!");
            }
        }
    }
}