using UnityEngine;
using System.Collections.Generic;

public class LightningBullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 10f;
    public float attackRate = 0.2f;
    
    private float _nextDamageTime;
    private List<EnemyEntityBase> _enemiesInZone = new List<EnemyEntityBase>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyEntityBase enemy = other.GetComponent<EnemyEntityBase>();
        if (enemy != null && !_enemiesInZone.Contains(enemy))
        {
            _enemiesInZone.Add(enemy);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyEntityBase enemy = other.GetComponent<EnemyEntityBase>();
        if (enemy != null && _enemiesInZone.Contains(enemy))
        {
            _enemiesInZone.Remove(enemy);
        }
    }
    
    private void Update()
    {
        if (Time.time >= _nextDamageTime)
        {
            DamageAllEnemies();
            _nextDamageTime = Time.time + attackRate;
        }
    }
    
    void DamageAllEnemies()
    {
        foreach (EnemyEntityBase enemy in _enemiesInZone)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(transform, Mathf.RoundToInt(damage));
                Debug.Log($"Молния ударила {enemy.name} на {damage} урона");
            }
        }
    }
}