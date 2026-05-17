using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed;
    public float _distance;
    public int _damage;
    public LayerMask _layerMask;

    private void Update()
    {
        RaycastHit2D other = Physics2D.Raycast(transform.position, transform.up, _distance, _layerMask);
        if (other.collider != null)
        {
            if (other.collider.TryGetComponent(out EnemyEntityBase enemy))
            {
                
                other.collider.GetComponent<EnemyEntityBase>().TakeDamage(transform, _damage);
                
                Destroy(gameObject);
            }

            if (other.collider.TryGetComponent(out EnemyRangedEntity RangedEnemy))
            {

                other.collider.GetComponent<EnemyRangedEntity>().TakeDamage(transform, _damage);

                Destroy(gameObject);
            }

            else
            {
                Destroy(gameObject);
            }
        }
        transform.Translate(Vector2.up * _speed *  Time.deltaTime);
    }
}

