using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float _speed;
    private int _damage;
    private Vector2 _direction;
    private bool _isInitialized;

    public void Initialize(Vector2 direction, float speed, int damage)
    {
        _direction = direction.normalized;
        _speed = speed;
        _damage = damage;
        _isInitialized = true;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        if (!_isInitialized) return;
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Train player))
        {
            player.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}