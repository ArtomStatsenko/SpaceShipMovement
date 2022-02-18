using UnityEngine;
using System;

public sealed class Asteroid : MonoBehaviour
{
    public event Action<Asteroid> OnDestroyedEvent;

    public void DestroyIfFarFromObject(Transform ship, float maxDistance)
    {
        float distance = Vector3.Magnitude(ship.transform.position - transform.position);

        if (distance > maxDistance)
        {
            OnDestroyedEvent?.Invoke(this);
            Destroy(gameObject);
        }
    }
}