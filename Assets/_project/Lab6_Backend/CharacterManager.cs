using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField idInput;
    public TMP_InputField emailInput;

    [Header("API Config")]
    // Đã sửa Port về 5165 khớp với Program.cs của bạn
    private string baseUrl = "http://localhost:5284/api/Character";

    // --- CÁC HÀM GỌI TỪ NÚT BẤM (UI) ---

    public void OnGetAllClick() => StartCoroutine(GetAllRoutine());
    public void OnGetByIdClick() => StartCoroutine(GetByIdRoutine());
    public void OnGetByEmailClick() => StartCoroutine(GetByEmailRoutine());

    // --- LOGIC XỬ LÝ API ---

    // 1. Lấy toàn bộ danh sách Characters
    IEnumerator GetAllRoutine()
    {
        Debug.Log("<color=yellow>Đang gọi API lấy danh sách...</color>");
        using (UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/get-all"))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string rawJson = req.downloadHandler.text;
                Debug.Log("Dữ liệu thô từ Server: " + rawJson);

                // Giải mã JSON thành Object
                var res = JsonUtility.FromJson<ResponseCharacterList>(rawJson);

                if (res != null && res.data != null && res.data.Count > 0)
                {
                    foreach (var c in res.data)
                    {
                        Debug.Log($"<color=orange>Character:</color> {c.name} (Lv.{c.level}) - Email: {c.email}");
                    }
                }
                else
                {
                    Debug.LogWarning("Server trả về danh sách trống hoặc sai định dạng JSON!");
                }
            }
            else
            {
                Debug.LogError($"Lỗi kết nối Get-All: {req.error} | URL: {req.url}");
            }
        }
    }

    // 2. Tìm Character theo ID
    IEnumerator GetByIdRoutine()
    {
        string id = idInput.text;
        if (string.IsNullOrEmpty(id)) { Debug.LogError("Vui lòng nhập ID!"); yield break; }

        using (UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/get-by-id/{id}"))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<ResponseCharacter>(req.downloadHandler.text);
                if (res.issuccess && res.data != null)
                {
                    Debug.Log($"<color=green>Tìm thấy ID {id}:</color> {res.data.name} - Máu: {res.data.health} - Level: {res.data.level}");
                }
                else
                {
                    Debug.LogWarning("Thông báo từ Server: " + res.notification);
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy Character với ID này hoặc lỗi Server!");
            }
        }
    }

    // 3. Tìm Character theo Email
    IEnumerator GetByEmailRoutine()
    {
        string email = emailInput.text;
        if (string.IsNullOrEmpty(email)) { Debug.LogError("Vui lòng nhập Email!"); yield break; }

        using (UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/get-by-email/{email}"))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<ResponseCharacter>(req.downloadHandler.text);
                if (res.issuccess && res.data != null)
                {
                    Debug.Log($"<color=cyan>Nhân vật của {email}:</color> {res.data.name} (Lv.{res.data.level})");
                }
                else
                {
                    Debug.LogWarning("Thông báo: " + res.notification);
                }
            }
            else
            {
                Debug.LogError("Lỗi: Email không tồn tại hoặc chưa có nhân vật!");
            }
        }
    }
}