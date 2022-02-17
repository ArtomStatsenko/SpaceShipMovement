﻿using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private bool _isCameraDisabled = false;
    [SerializeField] private float _mouseSensivity = 4f;
    [SerializeField] private float _scrollSensivity = 2f;
    [SerializeField] private float _orbitDampening = 3f;
    [SerializeField] private float _scrollDampening = 6f;
    [SerializeField] private float _zooming = 0.3f;
    [SerializeField] private float _maxAngleY = 90f;
    [SerializeField] private float _minAngleY = -90f;
    [SerializeField] private float _followingSpeed = 2f;
    [SerializeField] private GameObject _target;

    private Transform _cameraTransform;
    private Transform _cameraPivotTransform;
    private Vector3 _localRotation;
    private float _cameraDistance = 10f;
    private float _maxCameraDistance = 20f;
    private float _minCameraDistance = 5f;

    private void Start()
    {
        _cameraTransform = transform;
        _cameraPivotTransform = transform.parent;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isCameraDisabled = !_isCameraDisabled;
        }

        if (!_isCameraDisabled)
        {
            Rotate();

//#if UNITY_EDITOR
//            MouseClickRotate();
//#elif UNITY_ANDROID || UNITY_IOS
//            TouchRotate();
//#endif
            Zoom();
            FollowTarget();
        }
    }

    float angleX = 0;
    float angleY = 0;
    float angleXTemp = 0;
    float angleYTemp = 0;
    Vector3 firstPoint = Vector3.zero;
    Vector3 secondPoint = Vector3.zero;

    private void MouseClickRotate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPoint = Input.mousePosition;
            angleXTemp = angleX;
            angleYTemp = angleY;
        }

        if (Input.GetMouseButton(0))
        {
            secondPoint = Input.mousePosition;
            angleX = angleXTemp + (secondPoint.x - firstPoint.x) * 180 / Screen.width;
            angleY = angleYTemp + (secondPoint.y - firstPoint.y) * 180 / Screen.height;
            Quaternion rotation = Quaternion.Euler(angleY, angleX, 0f);
            _cameraPivotTransform.rotation = Quaternion.Lerp(_cameraPivotTransform.rotation, rotation, Time.deltaTime * _orbitDampening);
        }
    }

    private void TouchRotate()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            firstPoint = touch.position;
            angleXTemp = angleX;
            angleYTemp = angleY;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            secondPoint = touch.position;
            angleX = angleXTemp + (secondPoint.x - firstPoint.x) * 180 / Screen.width;
            angleY = angleYTemp + (secondPoint.y - firstPoint.y) * 180 / Screen.height;
            Quaternion rotation = Quaternion.Euler(angleY, angleX, 0f);
            _cameraPivotTransform.rotation = Quaternion.Lerp(_cameraPivotTransform.rotation, rotation, Time.deltaTime * _orbitDampening);
        }
    }

    private void Rotate()
    {
        float inputX = Input.GetAxis(AxisManager.MOUSE_X);
        float inputY = Input.GetAxis(AxisManager.MOUSE_Y);

        if (inputX != 0f || inputY != 0f)
        {
            _localRotation.x += inputX * _mouseSensivity;
            _localRotation.y -= inputY * _mouseSensivity;

            //if (_localRotation.y < _minAngleY)
            //{
            //    _localRotation.y = _minAngleY;
            //}
            //else if (_localRotation.y > _maxAngleY)
            //{
            //    _localRotation.y = _maxAngleY;
            //}
        }

        Quaternion rotation = Quaternion.Euler(_localRotation.y, _localRotation.x, 0f);
        _cameraPivotTransform.rotation = Quaternion.Lerp(_cameraPivotTransform.rotation, rotation, Time.deltaTime * _orbitDampening);
    } 
        
    private void Zoom()
    {
        float inputScrollWeel = Input.GetAxis(AxisManager.MOUSE_SCROLLWHEEL);

        if (inputScrollWeel != 0f)
        {
            float scrollAmount = inputScrollWeel * _scrollSensivity;
            scrollAmount *= (_cameraDistance * _zooming);
            _cameraDistance -= scrollAmount;
            _cameraDistance = Mathf.Clamp(_cameraDistance, _minCameraDistance, _maxCameraDistance);
        }

        if (_cameraTransform.localPosition.z != -_cameraDistance)
        {
            float positionZ = Mathf.Lerp(_cameraTransform.localPosition.z, -_cameraDistance, Time.deltaTime * _scrollDampening);
            _cameraTransform.localPosition = new Vector3(0f, _cameraTransform.localPosition.y, positionZ);
        }
    }

    private void FollowTarget()
    {
        Vector3 followDirection = _target.transform.position;
        _cameraPivotTransform.position = Vector3.Lerp(_cameraPivotTransform.position, followDirection, Time.deltaTime * _followingSpeed);
    }
}