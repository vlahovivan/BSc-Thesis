using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 1000.0f;
    public float rotateSpeed = 60.0f;
    public Transform target;
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private Vector3 moveDirection;
    private Vector3 currentRotation;

    private bool rotationChanged = false;
    
    void Start()
    {
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;

        moveDirection = target.position - transform.localPosition;
        moveDirection = Vector3.Normalize(moveDirection);

        moveDirection *= moveSpeed;

        currentRotation = new Vector3(pitch, yaw, 0.0f);
    }

    void FixedUpdate()
    { 
        if(Input.GetKey(KeyCode.S)) {
            pitch += rotateSpeed * Time.fixedDeltaTime;
            if(pitch > 90.0f) pitch = 90.0f;
            rotationChanged = true;
        }
        if(Input.GetKey(KeyCode.W)) {
            pitch -= rotateSpeed * Time.fixedDeltaTime;
            if(pitch < -90.0f) pitch = -90.0f;
            rotationChanged = true;
        }
        if(Input.GetKey(KeyCode.A)) {
            yaw -= rotateSpeed * Time.fixedDeltaTime;
            rotationChanged = true;
        }
        if(Input.GetKey(KeyCode.D)) {
            yaw += rotateSpeed * Time.fixedDeltaTime;
            rotationChanged = true;
        }
        if(Input.GetKey(KeyCode.K)) {
            transform.localPosition += moveDirection * Time.fixedDeltaTime;
        }
        if(Input.GetKey(KeyCode.L)) {
            transform.localPosition -= moveDirection * Time.fixedDeltaTime;
        }

        if(yaw > 180.0f) yaw -= 360.0f;
        if(pitch > 180.0f) pitch -= 360.0f;
        if(yaw <= -180.0f) yaw += 360.0f;
        if(pitch <= -180.0f) pitch += 360.0f;
        
        if(rotationChanged) {
            currentRotation.x = pitch;
            currentRotation.y = yaw;
            transform.eulerAngles = currentRotation;
        
            moveDirection = target.position - transform.localPosition;
            moveDirection = Vector3.Normalize(moveDirection);

            moveDirection *= moveSpeed;

            rotationChanged = false;
        }
    }
}

