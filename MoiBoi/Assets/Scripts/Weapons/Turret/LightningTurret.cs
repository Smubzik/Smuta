using UnityEngine;

public class LightningTurret : MonoBehaviour
{
    public static LightningTurret Instance { get; private set; }
    
    [Header("Turret Stats")]
    public float damage = 10f;
    public float _offset = 0f;
    public float range = 12f;
    public float attackRate = 0.2f;
    
    [Header("References")]
    public Transform firePoint;
    public GameObject lightningPrefab;
    public BoxCollider2D damageZone;
    
    private GameObject _currentLightning;
    private float _nextDamageTime;
    private float _currentAngle;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        if (damageZone != null)
        {
            damageZone.size = new Vector2(damageZone.size.x, range);
            damageZone.offset = new Vector2(0, range / 2f);
        }
    }
    
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        
        // Вычисляем угол для поворота турели
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _currentAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _currentAngle + _offset);
        
        if (Input.GetMouseButton(0))
        {
            ActivateLightning();
        }
        else
        {
            DeactivateLightning();
        }
    }
    
    void ActivateLightning()
    {
        if (firePoint == null) return;
        
        if (_currentLightning == null && lightningPrefab != null)
        {
            _currentLightning = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);
        }
        
        if (_currentLightning != null)
        {
            _currentLightning.transform.position = firePoint.position;
            // Поворачиваем молнию в направлении турели
            _currentLightning.transform.rotation = Quaternion.Euler(0f, 0f, _currentAngle + _offset - 90f);
            
            // Наносим урон через коллайдер (он уже на турели)
            if (Time.time >= _nextDamageTime)
            {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));
                foreach (Collider2D enemy in hitEnemies)
                {
                    EnemyEntityBase e = enemy.GetComponent<EnemyEntityBase>();
                    if (e != null)
                    {
                        e.TakeDamage(transform, Mathf.RoundToInt(damage));
                    }
                }
                _nextDamageTime = Time.time + attackRate;
            }
        }
    }
    
    void DeactivateLightning()
    {
        if (_currentLightning != null)
        {
            Destroy(_currentLightning);
            _currentLightning = null;
        }
    }
    
    public void UpdateRange(float newRange)
    {
        range = newRange;
        
        if (damageZone != null)
        {
            damageZone.size = new Vector2(damageZone.size.x, newRange);
            damageZone.offset = new Vector2(0, newRange / 2f);
            Debug.Log($"Коллайдер увеличен до {newRange}");
        }
    }
    
    public void DestroyTurret()
    {
        Destroy(gameObject);
    }
}