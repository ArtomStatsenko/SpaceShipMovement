using UnityEngine;

public sealed class Asteroid : MonoBehaviour
{
    private Transform _ship;
    private float _maxDistance = 200f;

    private void Start()
    {
        _ship = FindObjectOfType<ShipController>().transform;
    }

    private void Update()
    {
        if (_ship == null)
        {
            return;
        }

        float distance = Vector3.Magnitude(_ship.transform.position - transform.position);

        if (distance > _maxDistance)
        {
            Destroy(gameObject);
        }
    }
}