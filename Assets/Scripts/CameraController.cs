using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private float _mouseSensivity = 4f;
    [SerializeField] private float _scrollSensivity = 2f;
    [SerializeField] private float _orbitDampening = 3f;
    [SerializeField] private float _scrollDampening = 6f;
    [SerializeField] private float _zooming = 0.3f;
    [SerializeField] private float _followingSpeed = 7f;
    [SerializeField] private GameObject _target;

    private Transform _cameraTransform;
    private Transform _cameraPivotTransform;
    private Vector3 _localRotation;
    private float _cameraDistance = 10f;
    private float _maxCameraDistance = 20f;
    private float _minCameraDistance = 5f;
    float _angleX = 0;
    float _angleY = 0;
    float _angleXTemp = 0;
    float _angleYTemp = 0;
    Vector3 _firstPoint = Vector3.zero;
    Vector3 _secondPoint = Vector3.zero;


    private void Awake()
    {
        _cameraTransform = transform;
        _cameraPivotTransform = transform.parent;
    }

    //private void Start()
    //{
    //    _screenCenter.x = Screen.width * 0.5f;
    //    _screenCenter.y = Screen.height * 0.5f;
    //}

    private void LateUpdate()
    {
        //Rotate();

#if UNITY_EDITOR
        MouseClickRotate();
        Zoom();
#elif UNITY_ANDROID || UNITY_IOS
        TouchRotate();
#endif
        FollowTarget();
    }

    //private Vector2 _screenCenter;
    //private Vector2 _lookInput;
    //private Vector2 _mouseDistance;
    //private float _rotationSpeed = 90f;


    private void MouseClickRotate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstPoint = Input.mousePosition;
            _angleXTemp = _angleX;
            _angleYTemp = _angleY;
        }

        if (Input.GetMouseButton(0))
        {
            _secondPoint = Input.mousePosition;
            _angleX = _angleXTemp + (_secondPoint.x - _firstPoint.x) * 180f / Screen.width;
            _angleY = _angleYTemp + (_secondPoint.y - _firstPoint.y) * 180f / Screen.height;
            Quaternion rotation = Quaternion.Euler(_angleY, _angleX, 0f);
            _cameraPivotTransform.rotation = Quaternion.Lerp(_cameraPivotTransform.rotation, rotation, Time.deltaTime * _orbitDampening);
        }

        //_lookInput.x = Input.mousePosition.x;
        //_lookInput.y = Input.mousePosition.y;

        //_mouseDistance.x = (_lookInput.x - _screenCenter.x) / _screenCenter.y;
        //_mouseDistance.y = (_lookInput.y - _screenCenter.y) / _screenCenter.y;
        //_mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);
        //_mouseDistance *= _rotationSpeed;
        //_cameraPivotTransform.Rotate(_mouseDistance.x, _mouseDistance.y, 0f, Space.Self);

        ////if (Input.GetMouseButton(0))
        ////{
        ////    _secondPoint = Input.mousePosition;
        ////    _angleX = (_secondPoint.x - _screenCenter.x) * 180f / Screen.width;
        ////    _angleY = (_secondPoint.y - _screenCenter.y) * 180f / Screen.height;
        ////    Quaternion rotation = Quaternion.Euler(-_angleY, _angleX, 0f);
        ////    _cameraPivotTransform.rotation = Quaternion.Lerp(_cameraPivotTransform.rotation, rotation, Time.deltaTime * _orbitDampening);
        ////}
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
            _firstPoint = touch.position;
            _angleXTemp = _angleX;
            _angleYTemp = _angleY;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            _secondPoint = touch.position;
            _angleX = _angleXTemp + (_secondPoint.x - _firstPoint.x) * 180 / Screen.width;
            _angleY = _angleYTemp + (_secondPoint.y - _firstPoint.y) * 180 / Screen.height;
            Quaternion rotation = Quaternion.Euler(_angleY, _angleX, 0f);
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