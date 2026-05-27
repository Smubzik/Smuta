using UnityEngine;

public class Turret : MonoBehaviour
{
    public static Turret Instance { get; private set; }
    
    [Header("Turret Stats")]
    public float damage = 3f;
    public float _offset = 0f;
    private float _time;
    public float _startTime = 0.5f;
    
    [Header("Multi-shot")]
    public int extraProjectiles = 0;
    public float spreadAngle = 15f;
    
    [Header("References")]
    public GameObject _bullet;
    public Transform _point;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        
        // Вращение за мышкой
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + _offset); // ← -90 если спрайт смотрит вверх
        
        // Стрельба
        if (_time <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                _time = _startTime;
            }
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }
    
    void Shoot()
    {
        ShootBullet(_point, 0f);
        
        for (int i = 0; i < extraProjectiles; i++)
        {
            float angleOffset = (i + 1) * spreadAngle / (extraProjectiles + 1);
            ShootBullet(_point, angleOffset);
            ShootBullet(_point, -angleOffset);
        }
        
    }
    
    void ShootBullet(Transform point, float angleOffset)
    {
        if (point == null) return;
        
        // Создаём пулю с поворотом от турели
        float angle = transform.eulerAngles.z + angleOffset + 90f;
        GameObject bulletObj = Instantiate(_bullet, point.position, Quaternion.Euler(0, 0, angle));
        
        // Передаём урон
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.damage = damage;
        }
    }
    
    public void DestroyTurret()
    {
        Destroy(gameObject);
    }
}