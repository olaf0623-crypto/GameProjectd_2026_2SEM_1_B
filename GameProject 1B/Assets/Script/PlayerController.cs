using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;                          //애니메이터
    [SerializeField] private Transform cameraTransform;                  //캐릭터를 따라갈 카메라

    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 3f;                       //걷기 속도
    [SerializeField] private float runSpeed = 6f;                        //뛰기 속도
    [SerializeField] private float rotationSpeed = 10f;                  //회전 속도

    [Header("바닥 설정")]
    [SerializeField] private float gravity = -20f;                       //중력

    private CharacterController controller;                          //유니티의 캐릭터 컨트롤러
    private float verticalVelocity;                                  //속도 값

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        //1. WASD 입력
        Vector2 input = Vector2.zero;

        if (keyboard.aKey.isPressed)
            input.x -= 1f;
        if (keyboard.dKey.isPressed)
            input.x += 1f;
        if (keyboard.sKey.isPressed)
            input.y -= 1f;
        if (keyboard.wKey.isPressed)
            input.y += 1f;

        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // 카메라의 위아래 기울기는 이동에 사용 하지 않는다.
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        //4. Shift 달리기
        bool isRunning = keyboard.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //5. 수평 이동
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        //6. 이동 방향으로 회전
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //7. 기본 중력 설정
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        float animationSpeed = 0f;
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            animationSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("speed", animationSpeed, 0.1f, Time.deltaTime);

    }
}