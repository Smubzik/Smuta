using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageAmount;

    public event EventHandler OnSwordSwing;

    private PolygonCollider2D _polygonCollider2D;


    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }



    private void Start()
    {
        AttackColliderTurnOffOn();
    }
    public void Attack()
    {
        AttackColliderTurnOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntityBase enemyEntity))
        {
            enemyEntity.TakeDamage(transform, 5);
        }
    }


    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled=true;
    }

    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOn();
        AttackColliderTurnOff();
    }
}
