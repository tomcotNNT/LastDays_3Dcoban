using UnityEngine;

public class ActionState : StateMachineBehaviour
{
    private WeaponManager wm;
    private bool hasOpenedMovement; // Biến kiểm soát để chỉ chạy lệnh 1 lần

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wm == null) 
            wm = animator.GetComponentInParent<WeaponManager>();

        if (wm != null) 
        {
            wm.EnableMovement(0); // Khóa chân ngay khi vào đòn
        }
        
        hasOpenedMovement = false; // Reset trạng thái khi bắt đầu đòn mới
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Kiểm tra nếu đã qua 70% và chưa mở khóa thì mới gọi lệnh
        if (!hasOpenedMovement && stateInfo.normalizedTime > 0.7f)
        {
            if (wm != null) 
            {
                wm.EnableMovement(1);
                hasOpenedMovement = true; // Đánh dấu đã mở, không gọi lại nữa
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wm != null) 
        {
            wm.EnableMovement(1); // Đảm bảo luôn mở khóa khi thoát đòn đột ngột
        }
    }
}