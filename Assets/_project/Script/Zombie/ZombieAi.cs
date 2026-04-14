using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public NavMeshAgent agent;
    public Animator anim;
    public Transform[] waypoints;
    public Transform playerTarget;
    public float attackDistance = 2f; // Khoảng cách để đứng lại đánh

    [Header("Cài đặt tấn công")]
    public int zombieDamage = 15;
    
    private int currentWaypointIndex = 0;
    private AttributesManager zombieStats;
    private bool isDead = false;

    void Awake()
    {
        // Lấy script quản lý máu của chính con zombie này
        zombieStats = GetComponent<AttributesManager>();
    }

    void OnEnable()
    {
        if (zombieStats != null) zombieStats.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (zombieStats != null) zombieStats.OnDeath -= HandleDeath;
    }

    void Update()
    {
        // Nếu đã chết thì không làm gì cả
        if (isDead) return;

        if (playerTarget != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer <= attackDistance)
            {
                AttackPlayer();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetBool("isAttacking", false);
        agent.isStopped = false; // Đảm bảo agent tiếp tục chạy

        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void AttackPlayer()
    {
        agent.isStopped = true; // Dừng lại để thực hiện động tác đánh
        anim.SetBool("isAttacking", true);
        
        // Quay mặt về phía Player khi đánh
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // --- HÀM QUAN TRỌNG: GỌI TỪ ANIMATION EVENT CỦA ZOMBIE ---
    public void ZombieHitEvent()
    {
        if (playerTarget == null || isDead) return;

        // Tính lại khoảng cách lúc tay vung trúng
        float dist = Vector3.Distance(transform.position, playerTarget.position);
        if (dist <= attackDistance + 0.5f)
        {
            // Tìm AttributesManager trên Player để trừ máu
            AttributesManager playerStats = playerTarget.GetComponent<AttributesManager>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(zombieDamage);
                Debug.Log($"<color=orange>Zombie</color> cào Player mất {zombieDamage} HP");
            }
        }
    }

    void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        agent.isStopped = true;
        agent.enabled = false; // Tắt agent để không cản đường
        
        // Logic chết đã được AttributesManager.Die() xử lý một phần (Trigger anim Death)
        // Ở đây chúng ta chỉ đảm bảo script AI này ngừng hoạt động
    }
}