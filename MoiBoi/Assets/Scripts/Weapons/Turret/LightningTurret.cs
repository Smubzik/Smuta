using UnityEngine;

public class LightningTurret : MonoBehaviour
{
    public static LightningTurret Instance { get; private set; }
    
    [Header("Turret Stats")]
    public float damage = 10f;
    public float _offset = 0f;
    public float attackRate = 0.2f;
    
    [Header("References")]
    public Transform firePoint;
    public GameObject lightningPrefab;
    
    private GameObject _currentLightning;
    private LightningBullet _lightningDamage;
    private float _currentTurretAngle;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _currentTurretAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _currentTurretAngle + _offset);
        
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
            _lightningDamage = _currentLightning.GetComponent<LightningBullet>();
            
            if (_lightningDamage != null)
            {
                _lightningDamage.damage = damage;
                _lightningDamage.attackRate = attackRate;
            }
        }
        
        if (_currentLightning != null)
        {
            _currentLightning.transform.position = firePoint.position;
            _currentLightning.transform.rotation = Quaternion.Euler(0, 0, _currentTurretAngle + _offset - 180f);
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
    
    public void DestroyTurret()
    {
        Destroy(gameObject);
    }
}