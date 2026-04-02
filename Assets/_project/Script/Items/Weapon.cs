using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlash;
    public AudioSource shotSound;
    public float damage = 10f;
    public float range = 100f;

    void Update()
    {
        // Nhấn chuột trái để bắn
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        shotSound.Play();

        // Kỹ thuật Raycast để kiểm tra bắn trúng Zombie
        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, range))
        {
            Debug.Log("Trúng mục tiêu: " + hit.transform.name);
            // Sau này thêm code trừ máu Zombie ở đây
        }
    }
}