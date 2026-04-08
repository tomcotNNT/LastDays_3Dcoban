using UnityEngine;

public class AuthUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject accountPanel;
    public GameObject characterPanel;

    void Start()
    {
        ShowLogin(); // Khởi đầu game luôn là Login
    }

    // --- HÀM BỔ TRỢ: Tắt tất cả các bảng cùng lúc ---
    private void HideAllPanels()
    {
        if (loginPanel) loginPanel.SetActive(false);
        if (registerPanel) registerPanel.SetActive(false);
        if (accountPanel) accountPanel.SetActive(false);
        if (characterPanel) characterPanel.SetActive(false);
    }

    // --- HIỂN THỊ TỪNG BẢNG ---

    public void ShowLogin()
    {
        HideAllPanels();
        loginPanel.SetActive(true);
    }

    public void ShowRegister()
    {
        HideAllPanels();
        registerPanel.SetActive(true);
    }

    // --- SAU KHI LOGIN THÀNH CÔNG: Hiện cả Account và Character ---
    public void OnLoginSuccess()
    {
        HideAllPanels();
        accountPanel.SetActive(true);
        characterPanel.SetActive(true);
        
        Debug.Log("<color=cyan>Đã mở Dashboard Quản lý!</color>");
    }

    // Nếu muốn chuyển đổi lẻ giữa 2 bảng khi đang ở trong Game
    public void ShowAccountOnly()
    {
        HideAllPanels();
        accountPanel.SetActive(true);
    }

    public void ShowCharacterOnly()
    {
        HideAllPanels();
        characterPanel.SetActive(true);
    }
}