using UnityEngine;
using System;

public sealed class SpawnController : MonoBehaviour
{
    public event Action<Transform, float> OnCheckedDistanceEvent;

    [SerializeField] private Transform _asteroidSpawnPoint;
    [SerializeField] private float _spawnTimeDelay = 5f;
    [SerializeField] private float _checkDistanceTimeDelay = 10f;
    [SerializeField] private float _maxDistance = 200f;

    private AsteroidFactory _asteroidFactory;
    private float _nextSpawnTime;
    private float _nextCheckDistanceTime;
    private ShipController _ship;

    private void Awake()
    {
        _ship = FindObjectOfType<ShipController>();
    }

    private void Start()
    {
        _asteroidFactory = new AsteroidFactory(_asteroidSpawnPoint);
    }

    private void Update()
    {
        float time = Time.time;

        if (time > _nextSpawnTime)
        {
            Asteroid asteroid =_asteroidFactory.CreateAsteroid();
            OnCheckedDistanceEvent += asteroid.DestroyIfFarFromObject;
            asteroid.OnDestroyedEvent += RemoveListener;
            _nextSpawnTime = time + _spawnTimeDelay;
        }

        if (_ship != null && time > _nextCheckDistanceTime)
        {
            OnCheckedDistanceEvent?.Invoke(_ship.transform, _maxDistance);
            _nextCheckDistanceTime = time + _checkDistanceTimeDelay;
        }
    }

    private void RemoveListener(Asteroid asteroid)
    {
        OnCheckedDistanceEvent -= asteroid.DestroyIfFarFromObject;
        asteroid.OnDestroyedEvent -= RemoveListener;
    }
}
