using UnityEngine;

public sealed class ShipController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _turnSpeed = 2f;

    private Transform _shipTransform;
    private Transform _cameraTransform;

    private void Awake()
    {
        _shipTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
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
