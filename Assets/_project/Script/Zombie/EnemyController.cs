using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float walkSpeed = 1.5f; // Tốc độ chậm để Player dễ thở
    public float chaseRange = 10f; // Khoảng cách nhận diện Player
    public float stopDistance = 1.5f; // Dừng lại để cào cấu
    public float gravity = -20f;

    [Header("Components")]
    private CharacterController controller;
    private Animator anim;
    private Transform player;

    private float yVelocity;
    private float stateTimer;
    private bool isChasing;

   void Start()
{
    controller = GetComponent<CharacterController>();
    // Vì Animator nằm ở con (T-pose), phải dùng GetComponentInChildren
    anim = GetComponentInChildren<Animator>(); 
    player = GameObject.FindGameObjectWithTag("Player").transform;
    yVelocity = -1f;
}

    void Update()
    {
        HandleGravity();

        float distance = Vector3.Distance(transform.position, player.position);

        // LOGIC: Nếu ở gần và đang trong trạng thái Patrolling (đi bộ)
        if (distance < chaseRange && anim.GetBool("isPatrolling"))
        {
            ChasePlayer();
        }
        else
        {
            // Nếu không đuổi, vận tốc ngang bằng 0
            anim.SetFloat("Speed", 0);
        }
    }

    void HandleGravity()
    {
        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f;
        else
            yVelocity += gravity * Time.deltaTime;

        controller.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void ChasePlayer()
    {
        // 1. Tính hướng về phía Player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Zombie không bay lên trời

        // 2. Xoay Zombie nhìn về phía Player mượt mà
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // 3. Di chuyển thực tế (Sửa lỗi chạy tại chỗ)
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > stopDistance)
        {
            controller.Move(direction * walkSpeed * Time.deltaTime);
            anim.SetFloat("Speed", 1); // Kích hoạt animation Walk
        }
        else
        {
            anim.SetFloat("Speed", 0);
            // Có thể thêm anim.SetTrigger("Attack") ở đây sau này
        }
    }
}