using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float _knockbackForce = 3f;
    [SerializeField] private float _knockbackTimerMax = 0.3f;
    public bool IsGettingKnockbacked { get; private set; }

    private float _knockbackMovingTimer;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockbackMovingTimer -= Time.deltaTime;
        if (_knockbackMovingTimer < 0 )
        {
            StopknockBackMovement();
        }
    }

    public void GetKnockedBack(Transform damageSource) {
        IsGettingKnockbacked = true;
        _knockbackMovingTimer = _knockbackTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockbackForce / rb.mass;
        rb.AddForce( difference, ForceMode2D.Impulse );
    }
    public void StopknockBackMovement()
    {
        rb.linearVelocity = Vector2.zero;
        IsGettingKnockbacked = false;
    }
}
