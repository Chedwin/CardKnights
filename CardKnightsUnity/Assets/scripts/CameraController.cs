///
/// File Name:      CameraController.cs
/// 
/// Author:         Edwin Chen
/// Date Created:   Oct 19, 2017
/// Date Modified:  Oct 19, 2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    string rightStickX;
    string rightStickY;
    string circleBtn;

    public bool lockCursor;
    public float mouseSensitivity = 5.0f;
    public Transform target;
    public float distanceFromTarget = 2.0f;

    public float fastSmoothTime = 0.01f;

    public Vector2 pitchRange = new Vector2(-10, 85);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float pitch = 0.0f;
    float yaw = 0.0f;

	// Use this for initialization
	void Start () {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        rightStickX = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.RS_X);
        rightStickY = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.RS_Y);
        circleBtn = CKInputManager.Instance.GetCtrlKeyName(CKInputManager.PS4_CTRL.CIRCLE);
    }

    void CenterCameraBehindPlayer() {
        Vector3 behindPlayer = Vector3.zero;
        currentRotation = Vector3.SmoothDamp(currentRotation, target.forward, ref behindPlayer, fastSmoothTime);
        transform.position = target.position - transform.forward * distanceFromTarget;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate () {
        yaw += Input.GetAxis(rightStickX) * mouseSensitivity;
        pitch -= Input.GetAxis(rightStickY) * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchRange.x, pitchRange.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;

        if (Input.GetButtonUp(circleBtn))
            CenterCameraBehindPlayer();
    }
}