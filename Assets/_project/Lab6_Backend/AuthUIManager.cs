using UnityEngine;

public class AuthUIManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;

    // Hàm này chạy ngay khi bạn nhấn nút Play trong Unity
    void Start()
    {
        // THAY ĐỔI Ở ĐÂY: Gọi hàm hiện Register thay vì Login
        ShowRegister();
    }

    // Hàm để hiện màn hình Login và ẩn Register
    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    // Hàm để hiện màn hình Register và ẩn Login
    public void ShowRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }
}