using UnityEngine;

public sealed class ShipController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _turnSpeed = 2f;

    private Transform _shipTransform;
    private Transform _cameraTransform;

    private void Start()
    {
        _shipTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        //TODO get position and draw aim
        //    }
        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        //TODO get position
        //    }
        //    if (touch.phase == TouchPhase.Stationary)
        //    {
        //        //TODO get position
        //    }
        //}

        MoveShip();
        RotateShip();
    }

    private void MoveShip()
    {
        _shipTransform.position += _shipTransform.forward * _moveSpeed * Time.deltaTime;
    }

    private void RotateShip()
    {
        Quaternion rotation = Quaternion.LookRotation(_cameraTransform.forward);
        _shipTransform.rotation = Quaternion.Lerp(_shipTransform.rotation, rotation, _turnSpeed * Time.deltaTime);
    }
}
