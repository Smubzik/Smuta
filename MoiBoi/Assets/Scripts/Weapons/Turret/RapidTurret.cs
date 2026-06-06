using UnityEngine;

public class RapidTurret : MonoBehaviour
{
    public static RapidTurret Instance { get; private set; }
    
    [Header("Turret Stats")]
    public float damage = 2f;
    public float _offset = 0f;
    private float _time;
    public float _startTime = 0.1f;
    
    [Header("Multi-shot")]
    public int extraProjectiles = 0;
    public float spreadAngle = 15f;
    
    [Header("References")]
    public GameObject _bullet;
    public Transform _point1;
    public Transform _point2;
    public Transform _point3;
    
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
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + _offset);
        
        // Стрельба ПРИ ЗАЖАТОЙ ЛКМ
        if (Input.GetMouseButton(0))
        {
            if (_time <= 0f)
            {
                Shoot();
                _time = _startTime;
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }
        else
        {
            _time = 0f; // Сбрасываем таймер, когда отпустили ЛКМ
        }
    }
    
    void Shoot()
    {
        ShootBullet(_point1, 0f);
        ShootBullet(_point2, 0f);
        ShootBullet(_point3, 0f);
        
        for (int i = 0; i < extraProjectiles; i++)
        {
            float angleOffset = (i + 1) * spreadAngle / (extraProjectiles + 1);
            ShootBullet(_point1, angleOffset);
            ShootBullet(_point1, -angleOffset);
            ShootBullet(_point2, angleOffset);
            ShootBullet(_point2, -angleOffset);
            ShootBullet(_point3, angleOffset);
            ShootBullet(_point3, -angleOffset);
        }
    }
    
    void ShootBullet(Transform point, float angleOffset)
    {
        if (point == null) return;
        
        float angle = transform.eulerAngles.z + angleOffset + 90f;
        GameObject bulletObj = Instantiate(_bullet, point.position, Quaternion.Euler(0, 0, angle));
        
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