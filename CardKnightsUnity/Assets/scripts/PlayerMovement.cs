using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerCommand;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    string leftStickX;
    string leftStickY;
    string l2Btn;


    public float walkSpeed = 2;
    public float runSpeed = 6;

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

        leftStickX = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.LS_X);
        leftStickY = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.LS_Y);
        l2Btn = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.L2);
    }

    void InitPlyCtrl()
    {
        plyCtrl = new PS4Controller();
        plyCtrl.squareBtn.CommandFunc = SquareButton;
        plyCtrl.crossBtn.CommandFunc = CrossButton;
        plyCtrl.triangleBtn.CommandFunc = TriangleButton;
        plyCtrl.circleBtn.CommandFunc = CircleButton;
    }

    void SquareButton()
    {
        Debug.Log("You press the SQUARE button!");
    }
    void CrossButton()
    {
        Debug.Log("You press the X button!");
    }
    void TriangleButton()
    {
        Debug.Log("You press the TRIANGLE button!");
    }
    void CircleButton()
    {
        Debug.Log("You press the O button!");
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

        bool running = Input.GetButton(l2Btn);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        //float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
        //animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void ProcessInput()
    {
        if (Input.GetButtonUp(plyCtrl.squareBtn.inputKey))
        {
            plyCtrl.squareBtn.Execute();
        }
        else if (Input.GetButtonUp(plyCtrl.crossBtn.inputKey))
        {
            plyCtrl.crossBtn.Execute();
        }
        else if (Input.GetButtonUp(plyCtrl.triangleBtn.inputKey))
        {
            plyCtrl.triangleBtn.Execute();
        }
        else if (Input.GetButtonUp(plyCtrl.circleBtn.inputKey))
        {
            plyCtrl.circleBtn.Execute();
        }
    }

    // Update the Movement
    void Update()
    {
        ProcessInput();
        MovePlayer();
    }

} // end class PlayerController
