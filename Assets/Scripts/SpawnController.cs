using UnityEngine;

public sealed class SpawnController : MonoBehaviour
{
    [SerializeField] private Transform _asteroidSpawnPoint;
    [SerializeField] private float _spawnTimeDelay = 5f;

    private AsteroidFactory _asteroidFactory;
    private float _nextSpawnTime;

    private void Start()
    {
        _asteroidFactory = new AsteroidFactory(_asteroidSpawnPoint);
    }

    private void Update()
    {
        if (Time.time > _nextSpawnTime)
        {
            _asteroidFactory.CreateAsteroid();
            _nextSpawnTime = Time.time + _spawnTimeDelay;
        }
    }
}
