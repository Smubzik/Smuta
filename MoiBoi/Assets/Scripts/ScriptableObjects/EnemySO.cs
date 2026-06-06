using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    [Header("Stats")]
    public string enemyName;
    public int enemyHealth;
    public int enemyDamageAmount;

    public float _chasingSpeed;
    public bool _isAttackingEnemy = true;
    public float _attackRange = 6f;            // Дальность атаки
    public float _attackRate = 1.5f;

    public float _stopDistance = 4f;
    //Для Врага-Бомбы
    public float _explosionRadius;
    public float _explosionDelay;
    public GameObject _explosionEffect;

    // Стреляющий враг
     public float _projectileSpeed = 10f;


    //Лазерный враг
    public float _chargeTime = 0.8f;
    public Color _chargeColor = Color.yellow;
    public Color _fireColor = Color.red;

}
