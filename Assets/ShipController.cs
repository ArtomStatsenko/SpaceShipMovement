using UnityEngine;

public sealed class ShipController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _transform.position += _transform.up * _moveSpeed * Time.deltaTime;
    }
}
