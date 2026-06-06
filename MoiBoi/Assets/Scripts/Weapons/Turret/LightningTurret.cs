using UnityEngine;

public class LightningTurret : MonoBehaviour
{
    public static LightningTurret Instance { get; private set; }
    
    [Header("Turret Stats")]
    public float damage = 10f;
    public float _offset = 0f;
    public float range = 5f;
    public float attackRate = 0.2f;
    private float _baseRange;
    
    [Header("References")]
    public Transform firePoint;
    public GameObject lightningPrefab;
    public BoxCollider2D damageZone;
    public SpriteRenderer turretSprite;
    
    private GameObject _currentLightning;
    private float _nextDamageTime;
    private float _currentAngle;
    private Vector3 _originalSpriteScale;
    private Vector3 _originalLightningScale;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _baseRange = range;
        
        if (turretSprite != null)
        {
            _originalSpriteScale = turretSprite.transform.localScale;
        }
        
        if (lightningPrefab != null)
        {
            _originalLightningScale = lightningPrefab.transform.localScale;
        }
        
        if (damageZone != null)
        {
            damageZone.size = new Vector2(damageZone.size.x, _baseRange);
            damageZone.offset = new Vector2(0, 0);
        }
    }
    
    private void OnDisable()
    {
        // При выключении турели (переключение на другую) — уничтожаем молнию
        DeactivateLightning();
    }
    
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _currentAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _currentAngle + _offset);
        
        if (damageZone != null)
        {
            damageZone.transform.rotation = Quaternion.Euler(0, 0, _currentAngle + _offset);
        }
        
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
            
            float scaleFactor = range / _baseRange;
            _currentLightning.transform.localScale = new Vector3(
                _originalLightningScale.x,
                _originalLightningScale.y * scaleFactor,
                _originalLightningScale.z
            );
        }
        
        if (_currentLightning != null)
        {
            _currentLightning.transform.position = firePoint.position;
            _currentLightning.transform.rotation = Quaternion.Euler(0f, 0f, _currentAngle + _offset - 180f);
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
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time >= _nextDamageTime)
        {
            EnemyEntityBase enemy = other.GetComponent<EnemyEntityBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(transform, Mathf.RoundToInt(damage));
                _nextDamageTime = Time.time + attackRate;
            }
        }
    }
    
    public void UpdateRange(float newRange)
    {
        range = newRange;
        
        if (damageZone != null)
        {
            damageZone.size = new Vector2(damageZone.size.x, newRange);
            damageZone.offset = new Vector2(0, -newRange / 2f);
        }
        
        if (turretSprite != null)
        {
            float scaleFactor = newRange / _baseRange;
            turretSprite.transform.localScale = new Vector3(
                _originalSpriteScale.x,
                _originalSpriteScale.y * scaleFactor,
                _originalSpriteScale.z
            );
        }
        
        Debug.Log($"Дальность обновлена: {newRange}");
    }
    
    public void ResetRange()
    {
        range = _baseRange;
        
        if (damageZone != null)
        {
            damageZone.size = new Vector2(damageZone.size.x, _baseRange);
            damageZone.offset = new Vector2(0, 0);  // ← сброс в 0,0
            Debug.Log($"Коллайдер сброшен: size.y={_baseRange}, offset.y=0");
        }
    }
    
    public void DestroyTurret()
    {
        Destroy(gameObject);
    }
}