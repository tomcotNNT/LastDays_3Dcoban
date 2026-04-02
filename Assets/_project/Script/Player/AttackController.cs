using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void Update() {
        // Đòn 1: Chuột trái (Đá)
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            anim.SetTrigger("Attack1"); 
        }

        // Đòn 2: Phím F (Búa)
        if (Input.GetKeyDown(KeyCode.F)) {
            anim.SetTrigger("Attack2");
        }
    }
}