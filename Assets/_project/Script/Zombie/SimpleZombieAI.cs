using UnityEngine;
using UnityEngine.AI;

public class SimpleZombieAI : MonoBehaviour
{
    [Header("Cấu hình mục tiêu")]
    public string playerTag = "Player";
    private Transform target;
    private NavMeshAgent agent;
    private Animator anim;
    private AttributesManager myAttributes;

    [Header("Cấu hình tấn công")]
    public float attackRange = 2.0f; 
    public float attackRate = 1.5f; 
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myAttributes = GetComponent<AttributesManager>();
        
        // Tìm Animator ở Model con (T-pose)
        anim = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        // Đồng bộ khoảng cách dừng của NavMesh với tầm đánh
        if (agent != null)
        {
            agent.stoppingDistance = attackRange - 0.2f;
        }
    }

    void Update()
    {
        // KIỂM TRA ĐIỀU KIỆN SỐNG:
        // Nếu không có Player HOẶC Zombie đã hết máu (dùng thuộc tính Health)
        if (target == null || myAttributes == null || myAttributes.Health <= 0)
        {
            if (agent.enabled) agent.isStopped = true;
            return;
        }

        // 1. DI CHUYỂN
        agent.SetDestination(target.position);

        // 2. CẬP NHẬT ANIMATION WALK
        // magnitude lấy độ dài vector vận tốc để biết Zombie đang đi nhanh hay chậm
        float speed = agent.velocity.magnitude;
        if (anim != null)
        {
            anim.SetFloat("Speed", speed);
        }

        // 3. LOGIC TẤN CÔNG
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        
        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                AttackTarget();
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    void AttackTarget()
    {
        // Xoay mặt về phía Player mượt mà khi đánh
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Giữ Zombie đứng thẳng
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        // Kích hoạt animation Attack
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        // Gây sát thương lên Player
        myAttributes.DealDamage(target.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}