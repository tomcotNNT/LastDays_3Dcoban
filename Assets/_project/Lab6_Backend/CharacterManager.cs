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
    private string baseUrl = "http://localhost:5284/api/Character";

    // 1. Get All Characters
    public void OnGetAllClick() => StartCoroutine(GetAllRoutine());

    // 2. Get By Id
    public void OnGetByIdClick() => StartCoroutine(GetByIdRoutine());

    // 3. Get By Email
    public void OnGetByEmailClick() => StartCoroutine(GetByEmailRoutine());

    IEnumerator GetAllRoutine()
    {
        using (UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/get-all-characters"))
        {
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<ResponseCharacterList>(req.downloadHandler.text);
                foreach (var c in res.data) Debug.Log($"<color=orange>Character:</color> {c.name} (Lv.{c.level}) - Chủ: {c.email}");
            }
        }
    }

    IEnumerator GetByIdRoutine()
    {
        string id = idInput.text;
        using (UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/get-character-by-id/{id}"))
        {
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<ResponseCharacter>(req.downloadHandler.text);
                Debug.Log($"<color=green>Tìm thấy ID {id}:</color> {res.data.name} - Máu: {res.data.health}");
            }
            else Debug.LogError("Không tìm thấy Character với ID này!");
        }
    }

    IEnumerator GetByEmailRoutine()
    {
        string email = emailInput.text;
        using (UnityWebRequest req = UnityWebRequest.Get($"{baseUrl}/get-character-by-email/{email}"))
        {
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<ResponseCharacter>(req.downloadHandler.text);
                Debug.Log($"<color=cyan>Nhân vật của {email}:</color> {res.data.name}");
            }
            else Debug.LogError("Email này chưa tạo nhân vật!");
        }
    }
}