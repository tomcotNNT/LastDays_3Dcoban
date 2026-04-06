using UnityEngine;
using UnityEngine.AI; // Cần thư viện này để điều khiển NavMesh

public class SimpleZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Tự động tìm Player dựa trên Tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Chưa gắn Tag 'Player' cho nhân vật chính kìa bạn ơi!");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Ra lệnh cho Cube tiến về phía Player
            agent.SetDestination(playerTransform.position);
        }
    }
}