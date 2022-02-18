using UnityEngine;

public sealed class AsteroidFactory
{
    private const string PATH = "Asteroid";
    private const string ROOT = "[Asteroid]";
    private Transform _spawnPoint;
    private float _randomOffset = 50f;
    private float _minSize = 5f;
    private float _maxSize = 15f;
    private GameObject _root;

    public AsteroidFactory(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        _root = new GameObject(ROOT);
    }

    public Asteroid CreateAsteroid()
    {
        Asteroid asteroid = Object.Instantiate(Resources.Load<Asteroid>(PATH));
        asteroid.transform.localScale = Vector3.one * Random.Range(_minSize, _maxSize);
        float x = Random.Range(_spawnPoint.position.x - _randomOffset, _spawnPoint.position.x + _randomOffset);
        float y = Random.Range(_spawnPoint.position.y - _randomOffset, _spawnPoint.position.y + _randomOffset);
        float z = Random.Range(_spawnPoint.position.z - _randomOffset, _spawnPoint.position.z + _randomOffset);
        asteroid.transform.position = new Vector3(x, y, z);
        asteroid.transform.parent = _root.transform;
        return asteroid;
    }
}