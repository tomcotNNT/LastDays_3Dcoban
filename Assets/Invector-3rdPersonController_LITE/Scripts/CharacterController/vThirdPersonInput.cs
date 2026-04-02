using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;

        [Header("Combat Input")]
        public KeyCode attack3Input = KeyCode.E;      // Phím cho Attack 3
        public KeyCode ultimateInput = KeyCode.R;     // Phím cho Ulti

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;
        
        // Tham chiếu đến hệ thống thuộc tính của bạn
        private PlayerAttributes playerAttr;

        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
            playerAttr = GetComponent<PlayerAttributes>(); // Lấy script thuộc tính
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();
            cc.ControlLocomotionType();
            cc.ControlRotationType();
        }

        protected virtual void Update()
        {
            InputHandle();
            cc.UpdateAnimator();
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); 
        }

        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
            CombatInput(); // Thêm hàm xử lý chiến đấu
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();
            if (cc != null) cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindFirstObjectByType<vThirdPersonCamera>();
                if (tpCamera == null) return;
                tpCamera.SetMainTarget(this.transform);
                tpCamera.Init();
            }
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) return;
                cameraMain = Camera.main;
                cc.rotateTarget = cameraMain.transform;
            }

            if (cameraMain) cc.UpdateMoveDirection(cameraMain.transform);
            if (tpCamera == null) return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);
            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput)) cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput)) cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput)) cc.Sprint(false);
        }

        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions()) cc.Jump();
        }

        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        #endregion

        #region Combat Inputs

        protected virtual void CombatInput()
        {
            // Xử lý Attack 3 (Phím E)
            if (Input.GetKeyDown(attack3Input) && !cc.isJumping)
            {
                // Gọi Trigger hoặc SetInteger tùy theo Animator bạn đã chỉnh
                cc.animator.SetInteger("AttackID", 3);
                cc.animator.SetTrigger("Attack"); // Trigger chung của Invector để vào state Attack
            }

            // Xử lý AttackUntil (Phím R)
            if (Input.GetKeyDown(ultimateInput) && !cc.isJumping)
            {
                // Chỉ cho phép đánh khi playerAttr báo canUseUltimate = true
                if (playerAttr != null && playerAttr.canUseUltimate)
                {
                    cc.animator.SetInteger("AttackID", 4);
                    cc.animator.SetTrigger("Attack");
                    Debug.Log("<color=gold>KÍCH HOẠT CHIÊU CUỐI!</color>");
                }
                else
                {
                    Debug.Log("<color=red>Chưa đủ nội năng để dùng Ulti!</color>");
                }
            }
        }

        #endregion
    }
}