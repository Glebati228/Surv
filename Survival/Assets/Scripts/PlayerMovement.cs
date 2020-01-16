using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : IAction<Transform>
{
    public float speed = 10f;
    public Transform camera;

    public void DoAction(Transform param)
    {
        if (Input.GetKey(KeyCode.W))
            param.position += camera.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            param.position -= camera.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            param.position += Vector3.Cross(camera.forward, Vector3.up).normalized * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            param.position -= Vector3.Cross(camera.forward, Vector3.up).normalized * speed * Time.deltaTime;

    }
}

public class PlayerCameraRotation : IAction<Transform>
{
    private float yaw = -90.0f;
    private float pitch = 0.0f;

    public float sensetivity = 0.1f;

    private Vector3 lastCursorPos;

    public void DoAction(Transform param)
    {
        float x = (lastCursorPos - Input.mousePosition).x;
        float y = (Input.mousePosition - lastCursorPos).y;

        lastCursorPos = Input.mousePosition;

        x *= sensetivity;
        y *= sensetivity;

        yaw += x;
        pitch += y;

        if (pitch > 89f)
            pitch = 89f;
        if (pitch < -89f)
            pitch = -89f;

        Vector3 camDirection;

        camDirection.x = Mathf.Cos(Mathf.Deg2Rad * yaw) * Mathf.Cos(Mathf.Deg2Rad * pitch);
        camDirection.y = Mathf.Sin(Mathf.Deg2Rad * pitch);
        camDirection.z = Mathf.Sin(Mathf.Deg2Rad * yaw) * Mathf.Cos(Mathf.Deg2Rad * pitch);

        camDirection = camDirection.normalized;

        param.LookAt(camDirection + param.position, Vector3.up);
    }
}
