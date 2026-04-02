using UnityEngine;
using Invector.vCharacterController;

public class WeaponManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private vThirdPersonController tcp;

    private Collider weaponCollider;

    void Awake()
    {
        if (weapon != null)
            weaponCollider = weapon.GetComponent<Collider>();
        
        if (tcp == null)
            tcp = GetComponentInParent<vThirdPersonController>();
    }

    // Đã thống nhất dùng int: 1 = Cho phép đi, 0 = Khóa
    public void EnableMovement(int isEnable)
    {
        if (tcp == null) return;
        
        bool canMove = (isEnable == 1);
        tcp.lockMovement = !canMove; 
        tcp.lockRotation = !canMove;
        
        // Log này giúp bạn kiểm tra dưới Console xem Event có chạy không
        Debug.Log($"[Movement] {(canMove ? "MỞ KHÓA" : "KHÓA CHÂN")}");
    }

    // Đã thống nhất dùng int: 1 = Bật kiếm, 0 = Tắt kiếm
    public void EnableWeaponCollider(int isEnable)
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = (isEnable == 1);
            Debug.Log($"[Weapon] {(isEnable == 1 ? "BẬT KIẾM" : "TẮT KIẾM")}");
        }
    }
}