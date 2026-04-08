using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class Register : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;

    // Tham chiếu đến AuthUIManager để điều khiển ẩn/hiện bảng
    public AuthUIManager uiManager;

    [Header("API Config")]
    // Port 5284 khớp với cấu hình Lab 6 của bạn
    private string registerUrl = "http://localhost:5284/api/Account/register";

    // 1. Khi nhấn nút Register trên giao diện
    public void OnRegisterClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string name = nameInput.text;

        // Kiểm tra nhập liệu
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("Vui lòng nhập đầy đủ thông tin!");
            return;
        }

        // Tạo object Account
        Account account = new Account
        {
            email = email,
            password = password,
            userName = name
        };

        string json = JsonUtility.ToJson(account);
        Debug.Log("Gửi dữ liệu đăng ký: " + json);

        StartCoroutine(PostRoutine(json));
    }

    // 2. Coroutine gửi yêu cầu API
    IEnumerator PostRoutine(string json)
    {
        // Sử dụng 'using' để tự động giải phóng bộ nhớ sau khi xong
        using (UnityWebRequest request = new UnityWebRequest(registerUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi Server: " + request.error);
                Debug.LogError("Chi tiết: " + request.downloadHandler.text);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Server phản hồi: " + responseText);

                // Chuyển JSON thành Object Response (Trong file Response.cs của bạn)
                Response res = JsonUtility.FromJson<Response>(responseText);

                if (res.issuccess)
                {
                    Debug.Log("<color=green>Đăng ký thành công cho:</color> " + res.data.email);
                    
                    // TỰ ĐỘNG CHUYỂN BẢNG: Gọi hàm ShowLogin từ UIManager
                    if (uiManager != null)
                    {
                        uiManager.ShowLogin();
                    }
                }
                else
                {
                    // Hiển thị thông báo lỗi từ Backend (ví dụ: Email đã tồn tại)
                    Debug.LogWarning("Thất bại: " + res.notification);
                }
            }
        }
    }

    // Nút bấm thủ công để quay lại màn hình Login
    public void GotoLogin()
    {
        if (uiManager != null) uiManager.ShowLogin();
    }
}