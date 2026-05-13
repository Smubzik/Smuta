using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] _enemy;
    public GameObject[] _spawnPoint;

    private int _rand;
    private int _randPosition;
    public float _startTimeBtwSpawns;
    private float _timeBtwSpawns;

    private void Start()
    {
        _timeBtwSpawns = _startTimeBtwSpawns;
    }

    private void Update()
    {
        if (_timeBtwSpawns <= 0)
        {
            _rand = Random.Range(0, _enemy.Length);
            _randPosition = Random.Range(0, _spawnPoint.Length);
            Instantiate(_enemy[_rand], _spawnPoint[_randPosition].transform.position, Quaternion.identity);
            _timeBtwSpawns = _startTimeBtwSpawns;
        }
        else
        {
            _timeBtwSpawns -= Time.deltaTime;
        }
    }
}
