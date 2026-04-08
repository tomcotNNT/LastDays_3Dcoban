using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class Login : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    // THIẾU CÁI NÀY: Tham chiếu đến Script quản lý ẩn/hiện bảng
    public AuthUIManager uiManager;

    [Header("API Config")]
    private string loginUrl = "http://localhost:5284/api/Account/login";

    // Chuyển sang bảng Register (Dùng trong cùng 1 Scene)
    public void GotoRegister()
    {
        if (uiManager != null)
        {
            uiManager.ShowRegister();
        }
        else
        {
            // Nếu bạn dùng Scene riêng biệt thì mới dùng dòng này:
            // SceneManager.LoadScene("Register");
        }
    }

    // Khi người dùng nhấn nút Login
    public void OnLoginClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Vui lòng nhập đầy đủ Email và Mật khẩu!");
            return;
        }

        Account account = new Account
        {
            email = email,
            password = password
        };

        string json = JsonUtility.ToJson(account);
        StartCoroutine(OnLoginRoutine(json));
    }

    IEnumerator OnLoginRoutine(string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi từ Server: " + request.error);
                Debug.LogError("Chi tiết: " + request.downloadHandler.text);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Response response = JsonUtility.FromJson<Response>(responseText);

                if (response.issuccess)
                {
                    Debug.Log("Đăng nhập thành công!");
                    if (uiManager != null)
                    {
                        // Gọi hàm mới chúng ta vừa viết trong AuthUIManager
                        uiManager.OnLoginSuccess();
                    }
                }
                else
                {
                    Debug.LogWarning("Đăng nhập thất bại: " + response.notification);
                }
            }
        }
    }
}