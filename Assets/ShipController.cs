using UnityEngine;

public sealed class ShipController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Transform _movementTarget;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (_movementTarget == null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                //TODO get position and draw aim
            }
            if (touch.phase == TouchPhase.Moved)
            {
                //TODO get position
            }
            if (touch.phase == TouchPhase.Stationary)
            {
                //TODO get position
            }
        }

        Vector3 movement = _movementTarget.position - _transform.position;
        _transform.position += movement * _moveSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.LookRotation(movement);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, _turnSpeed * Time.deltaTime);
    }
}
