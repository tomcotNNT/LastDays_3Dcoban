using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Bắt buộc để dùng TMP_InputField
using UnityEngine.Networking;
using System.Text;

public class Accounts : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput; // Kéo ô Input_SearchEmail vào đây

    [Header("API Config")]
    private string baseUrl = "http://localhost:5284/api/Account"; // Port 5284 theo Swagger của bạn

    // 1. Thực hiện GetAllAccounts (Gán vào nút Btn_GetAll)
    public void GetAllAccounts()
    {
        StartCoroutine(GetAll());
    }

    // 2. Thực hiện GetAccountByEmail (Gán vào nút Btn_GetByEmail)
    public void GetAccountByEmail()
    {
        if (string.IsNullOrEmpty(emailInput.text))
        {
            Debug.LogWarning("Vui lòng nhập email cần tìm!");
            return;
        }
        StartCoroutine(GetByEmail());
    }

    // --- Phương thức thực hiện gọi API ---

    IEnumerator GetAll()
    {
        string url = $"{baseUrl}/get-all-accounts";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi: " + request.error);
            }
            else
            {
                ResponseAccountList response = JsonUtility.FromJson<ResponseAccountList>(request.downloadHandler.text);
                Debug.Log("Thông báo: " + response.notification);

                if (response.issuccess)
                {
                    foreach (var acc in response.data)
                    {
                        Debug.Log($"<color=cyan>User:</color> {acc.userName} | <color=yellow>Email:</color> {acc.email}");
                    }
                }
            }
        }
    }

    IEnumerator GetByEmail()
    {
        string email = emailInput.text;
        string url = $"{baseUrl}/get-account-by-email/{email}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi: " + request.error);
            }
            else
            {
                Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
                Debug.Log("Thông báo: " + response.notification);

                if (response.issuccess)
                {
                    Debug.Log($"<color=green>Tìm thấy:</color> {response.data.userName} ({response.data.email})");
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy tài khoản này.");
                }
            }
        }
    }
}