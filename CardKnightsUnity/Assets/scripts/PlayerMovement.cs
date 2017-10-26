using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerCommand;


public class PlayerMovement : MonoBehaviour
{
    string leftStickX;
    string leftStickY;
    string l2Btn;


    public float walkSpeed  = 6.0f;
    public float runSpeed   = 12.0f;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    Transform cameraT;
    Animator animator;

    PS4Controller plyCtrl;

    CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Start()
    {
        InitPlyCtrl();

        cameraT = Camera.main.transform;


    }

    void InitPlyCtrl()
    {
        leftStickX = CKInputManager.Instance.GetCtrlKeyName(PS4Controller.PS4_CTRL_MAP.LS_X);
        leftStickY = CKInputManager.Instance.GetCtrlKeyName(PS4Controller.PS4_CTRL_MAP.LS_Y);
        CKInputManager.Instance.SetPS4BtnCtrl(PS4Controller.PS4_CTRL_MAP.L2, SprintL2Button);
    }

    float SprintL2Button()
    {
        return 1.0f;   
    }

    private void MovePlayer()
    {
        float x = Input.GetAxis(leftStickX);
        float y = Input.GetAxis(leftStickY);

        Vector2 input = new Vector2(x, y);

        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = CKInputManager.Instance.IsPS4BtnDown(PS4Controller.PS4_CTRL_MAP.L2);

        if (running)
            Debug.Log("Sprinting!");

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        //float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
        //animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    // Update the Movement
    void Update()
    {
        MovePlayer();
    }

} // end class PlayerController
