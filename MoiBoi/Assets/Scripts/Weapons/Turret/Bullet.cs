using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 3f;
    public float speed = 20f;
    public float maxDistance = 0.5f;
    public LayerMask enemyLayer;
    
    private Rigidbody2D rb;
    private bool _hasHit = false;  // ← Флаг защиты от двойного урона
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 2f);
    }
    
    private void Update()
    {
        if (_hasHit) return;  // ← Если уже попали, выходим
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxDistance, enemyLayer);
        if (hit.collider != null)
        {
            _hasHit = true;  // ← Помечаем, что уже попали
            
            // Попытка получить компонент EnemyEntityBase (родительский)
            EnemyEntityBase enemy = hit.collider.GetComponent<EnemyEntityBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(transform, (int)damage);
                Destroy(gameObject);
                return;
            }
            
            // Если не нашли — уничтожаем пулю
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasHit)
        {
            _hasHit = true;
            Destroy(gameObject);
        }
    }
}