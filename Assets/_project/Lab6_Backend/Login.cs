using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class Login : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    // Thay đổi PORT cho đúng với API của bạn (5284)
    private string loginUrl = "http://localhost:5284/api/Account/login";

    public void GotoRegister()
    {
        // Chuyển sang Scene Register (Đảm bảo bạn đã add Scene này vào Build Settings)
        SceneManager.LoadScene("Register");
    }

    public void OnLoginClick()
    {
        var email = emailInput.text;
        var password = passwordInput.text;

        var account = new Account { email = email, password = password };
        var json = JsonUtility.ToJson(account);

        StartCoroutine(OnLoginRoutine(json));
    }

    IEnumerator OnLoginRoutine(string json)
    {
        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Lỗi kết nối: " + request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            Debug.Log(response.notification);

            if (response.issuccess)
            {
                Debug.Log("Chào mừng chiến binh: " + response.data.userName);
                
                // LƯU Ý: Đăng nhập xong nên chuyển vào Scene Game chính
                // Thay "MainGame" bằng tên Scene chơi game của bạn
                SceneManager.LoadScene("MainGame"); 
            }
            else
            {
                Debug.LogWarning("Đăng nhập thất bại: " + response.notification);
            }
        }
    }
}