using UnityEngine;

public sealed class ShipController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _turnSpeed = 2f;
    //[SerializeField] private Transform _movementTarget;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
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
        transform.position += transform.forward * _moveSpeed * Time.deltaTime;
    }

    private void RotateShip()
    {
        Quaternion rotation = Quaternion.LookRotation(_camera.transform.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _turnSpeed * Time.deltaTime);
    }
}
