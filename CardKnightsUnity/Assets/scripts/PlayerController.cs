using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
    string pause;
    void Start()
    {
        //animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;

        leftStickX = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.LS_X);
        leftStickY = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.LS_Y);
        l2Btn = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.L2);
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

    void Update()
    {
        MovePlayer();
    }

} // end class PlayerController
