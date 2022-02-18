using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 60f;
    [SerializeField] private float _scrollSensivity = 2f;
    [SerializeField] private float _scrollDampening = 6f;
    [SerializeField] private float _zooming = 0.3f;
    [SerializeField] private float _followingSpeed = 7f;
    [SerializeField] private GameObject _target;

    private float _cameraDistance = 10f;
    private float _maxCameraDistance = 20f;
    private float _minCameraDistance = 5f;
    private Vector2 _screenCenter;
    private Vector2 _lookInput;
    private Vector2 _mouseDistance;
    private Transform _cameraTransform;
    private Transform _cameraPivotTransform;

    private void Awake()
    {
        _cameraTransform = transform;
        _cameraPivotTransform = transform.parent;
    }

    private void Start()
    {
        _screenCenter.x = Screen.width * 0.5f;
        _screenCenter.y = Screen.height * 0.5f;
    }

    private void LateUpdate()
    {
#if UNITY_EDITOR
        MouseRotate();
        Zoom();
#elif UNITY_ANDROID || UNITY_IOS
        TouchRotate();
#endif
        FollowTarget();
    }

    private void MouseRotate()
    {
        if (Input.GetMouseButton(0))
        {
            Rotate(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void TouchRotate()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved)
        {
            Rotate(touch.position.x, touch.position.y);
        }
    }

    private void Rotate(float x, float y)
    {
        _lookInput.x = x;
        _lookInput.y = y;
        _mouseDistance.x = (_lookInput.x - _screenCenter.x) / _screenCenter.y;
        _mouseDistance.y = (_lookInput.y - _screenCenter.y) / _screenCenter.y;
        _mouseDistance *= _rotationSpeed * Time.deltaTime;
        Vector3 rotation = new Vector3(-_mouseDistance.y, _mouseDistance.x, 0f);
        _cameraPivotTransform.Rotate(rotation, Space.Self);
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