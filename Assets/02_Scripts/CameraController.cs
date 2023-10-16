using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public Transform target;

    public float speed = 10.0f;
    public float rotateSpeed = 2.0f;

    public float xLimitUp;
    public float xLimitDown;
    public float yLimitUp;
    public float yLimitDown;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        LimitCheck();
        Zoom();
        Rotate();
    }

    public void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * speed;

        if (_camera.fieldOfView <= 20.0f && scroll < 0)
        {
            _camera.fieldOfView = 20.0f;
        }
        else if(_camera.fieldOfView >= 60.0f && scroll > 0)
        {
            _camera.fieldOfView = 60.0f;
        }
        else
        {
            _camera.fieldOfView += scroll;
        }
    }

    public void Rotate()
    {
        if(Input.GetMouseButton(1))
        {
            Vector3 dir = transform.rotation.eulerAngles;
            dir.y += Input.GetAxis("Mouse X") * rotateSpeed;
            dir.x -= Input.GetAxis("Mouse Y") * rotateSpeed;

            if(dir.y >= xLimitUp)
            {
                dir.y = xLimitUp;
            }
            else if(dir.y <= xLimitDown)
            {
                dir.y = xLimitDown;
            }

            if(dir.x >= yLimitUp)
            {
                dir.x = yLimitUp;
            }
            else if(dir.x <= yLimitDown)
            {
                dir.x = yLimitDown;
            }

            Quaternion q = Quaternion.Euler(dir);
            dir.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.5f);
        }
    }

    public void LimitCheck()
    {
        if (_camera.fieldOfView == 60.0f)
        {
            xLimitUp = 70.0f;
            xLimitDown = 0.0f;
            yLimitUp = 70.0f;
            yLimitDown = 0.0f;
        }
        else if (_camera.fieldOfView == 45.0f)
        {
            xLimitUp = 80.0f;
            xLimitDown = 0.0f;
            yLimitUp = 80.0f;
            yLimitDown = 0.0f;
        }
        else if (_camera.fieldOfView == 30.0f)
        {
            xLimitUp = 90.0f;
            xLimitDown = 0.0f;
            yLimitUp = 90.0f;
            yLimitDown = 0.0f;
        }
    }
}
