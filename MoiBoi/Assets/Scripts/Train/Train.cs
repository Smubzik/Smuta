using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


[SelectionBase]
public class Train : MonoBehaviour
{

    public static Train Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerFq;
    public event EventHandler OnPlayerSq;

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    Vector2 inputVector;


    private Rigidbody2D rb;

    private float MinMovingSpeed = 0.1f;
    private bool IsMoving = false;
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _fq;
    private bool _sq;
    private bool _isAlive;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
    }
    private void Update()
    {
        inputVector = TrainGameInput.Instance.GetMovementVector();
    
    }

    private void FixedUpdate()
    {
        //if (_knockback.IsGettingKnockbacked)
           // return;
        HandleMovement();
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            Debug.Log(_currentHealth);
            StartCoroutine(DamageRecoveryRoutine());
        }
        DetectFquarter();
        DetectSquarter();
        DetectDeath();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive) { 
            _isAlive = false;
            TrainGameInput.Instance.DisableMovement();
            Turret.Instance.Destroy();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }

    }

    private void DetectFquarter()
    {
        if (_currentHealth <  (_maxHealth / 4) * 3 && _currentHealth >= (_maxHealth / 4) * 2)
            OnPlayerFq?.Invoke(this, EventArgs.Empty);

    }

    private void DetectSquarter()
    {
        if (_currentHealth < (_maxHealth / 4) * 2 && _currentHealth >= (_maxHealth / 4))
            OnPlayerSq?.Invoke(this, EventArgs.Empty);

    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > MinMovingSpeed || Mathf.Abs(inputVector.y) > MinMovingSpeed)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    
    public bool isMoving()
    {
        return IsMoving;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 PlayerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return PlayerScreenPosition;
    }
};
