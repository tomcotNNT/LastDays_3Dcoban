using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking; // Bắt buộc để dùng UnityWebRequest
using System.Collections;
using System.Text; // Để dùng Encoding
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;

    // Tham chiếu đến AuthUIManager để chuyển đổi bảng UI (Nếu bạn có)
    public AuthUIManager uiManager;

    [Header("API Config")]
    // Thay đổi port 5155 thành đúng port mà Web API (Lab 6) của bạn đang chạy
    private string registerUrl = "http://localhost:5284/api/Account/register";
    // 1. Khi nhấn nút Register trên giao diện
    public void OnRegisterClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string name = nameInput.text;

        // Kiểm tra sơ bộ xem người dùng đã nhập đủ chưa
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Vui lòng nhập đầy đủ Email và Mật khẩu!");
            return;
        }

        // Tạo object Account (khớp với class trong Response.cs)
        Account account = new Account
        {
            email = email,
            password = password,
            userName = name
        };

        // Chuyển đổi object thành chuỗi JSON
        string json = JsonUtility.ToJson(account);
        Debug.Log("Gửi JSON: " + json);

        // Bắt đầu quá trình gửi dữ liệu lên Server
        StartCoroutine(Post(json));
    }

    // 2. Hàm thực hiện gửi yêu cầu API (Coroutine)
    IEnumerator Post(string json)
    {
        // Tạo yêu cầu POST với dữ liệu JSON
        var request = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Cực kỳ quan trọng: Khai báo cho Server biết đây là dữ liệu JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Gửi và đợi phản hồi
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            // Trường hợp lỗi (Sai URL, Server chưa bật, Email đã tồn tại...)
            Debug.LogError("Lỗi từ Server: " + request.error);
            Debug.LogError("Chi tiết: " + request.downloadHandler.text);
        }
        else
        {
            // Trường hợp thành công
            string responseText = request.downloadHandler.text;
            Debug.Log("Server phản hồi: " + responseText);

            // Đọc dữ liệu JSON trả về thành Object Response
            Response res = JsonUtility.FromJson<Response>(responseText);

            if (res.issuccess)
            {
                Debug.Log("Đăng ký thành công cho: " + res.data.email);
                // Sau khi đăng ký xong, chuyển sang màn hình Login
                if (uiManager != null) uiManager.ShowLogin();
            }
            else
            {
                Debug.LogWarning("Thông báo: " + res.notification);
            }
        }
    }

    // Hàm bổ trợ để chuyển màn hình
    public void GotoLogin()
    {
        if (uiManager != null) uiManager.ShowLogin();
    }
}