using System;
using System.Collections;
using UnityEngine;
using TMPro;

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
    private Collider2D playerCollider;

    private float MinMovingSpeed = 0.1f;
    private bool IsMoving = false;
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    private float _baseMovingSpeed;
    private int _baseMaxHealth;
    
    [Header("Upgrade Stats")]
    public float armor = 0f;
    public float healthRegeneration = 0f;
    private float regenerationTimer = 0f;
    
    [Header("UI")]
    public TextMeshProUGUI healthText;
    
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public float MovingSpeed => movingSpeed;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        
        _baseMovingSpeed = movingSpeed;
        _baseMaxHealth = _maxHealth;
    }

    private void Start()
    {
        _maxHealth = _baseMaxHealth + GameData.BonusMaxHealth;
        _currentHealth = Mathf.Min(GameData.SavedHealth, _maxHealth);
        if (_currentHealth <= 0) _currentHealth = _maxHealth;
        
        _canTakeDamage = true;
        _isAlive = true;
        
        UpdateHealthUI();
    }
    
    private void Update()
    {
        if (TrainGameInput.Instance == null) 
        {
            return;
        }
        inputVector = TrainGameInput.Instance.GetMovementVector();
        
        if (healthRegeneration > 0 && _currentHealth < _maxHealth && _currentHealth > 0)
        {
            regenerationTimer += Time.deltaTime;
            if (regenerationTimer >= 1f)
            {
                regenerationTimer = 0f;
                Heal(Mathf.RoundToInt(healthRegeneration));
            }
        }
        
        UpdateHealthUI();
        
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }



    public void TakeDamage(int damage)
    {
        
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            
            int finalDamage = damage;
            if (armor > 0)
            {
                float reduction = armor / (armor + 10f);
                finalDamage = Mathf.Max(1, Mathf.RoundToInt(damage * (1 - reduction)));
            }
            
            _currentHealth = Mathf.Max(0, _currentHealth - finalDamage);
            GameData.SavedHealth = _currentHealth;
            
            Debug.Log($"Урон получен! Здоровье: {_currentHealth}/{_maxHealth}");
            
            UpdateHealthUI();
            StartCoroutine(DamageRecoveryRoutine());
        }
        DetectFquarter();
        DetectSquarter();
        DetectDeath();
    }
    
    public void Heal(int amount)
    {
        if (!_isAlive) return;
        
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        GameData.SavedHealth = _currentHealth;
        UpdateHealthUI();
        Debug.Log($"Вылечено {amount}. Здоровье: {_currentHealth}/{_maxHealth}");
    }
    
    public void SetHealth(int health)
    {
        _currentHealth = Mathf.Min(health, _maxHealth);
        GameData.SavedHealth = _currentHealth;
        UpdateHealthUI();
    }
    
    public void UpgradeMaxHealth(int bonus)
    {
        _maxHealth += bonus;
        GameData.BonusMaxHealth += bonus;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        GameData.SavedHealth = _currentHealth;
        UpdateHealthUI();
        Debug.Log($"Макс. здоровье увеличено до {_maxHealth}");
    }
    
    public void UpgradeSpeed(float bonus)
    {
        movingSpeed += bonus;
        GameData.BonusSpeed += bonus;
    }
    
    public void UpgradeArmor(float bonus)
    {
        armor += bonus;
        GameData.BonusArmor += bonus;
        Debug.Log($"Броня: {armor}");
    }
    
    public void UpgradeRegeneration(float bonus)
    {
        healthRegeneration += bonus;
        GameData.BonusRegeneration += bonus;
    }
    
    public void ResetToBaseStats()
    {
        movingSpeed = _baseMovingSpeed;
        _maxHealth = _baseMaxHealth;
        armor = 0f;
        healthRegeneration = 0f;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        GameData.SavedHealth = _currentHealth;
        UpdateHealthUI();
    }
    
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{_currentHealth}/{_maxHealth}";
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0 && _isAlive)
        { 
            _isAlive = false;
            TrainGameInput.Instance.DisableMovement();
            if (Turret.Instance != null)
                Turret.Instance.DestroyTurret();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            Debug.Log("Игрок умер!");
        }
    }

    private void DetectFquarter()
    {
        int quarter = _maxHealth / 4;
        if (_currentHealth < quarter * 3 && _currentHealth >= quarter * 2)
            OnPlayerFq?.Invoke(this, EventArgs.Empty);
    }

    private void DetectSquarter()
    {
        int quarter = _maxHealth / 4;
        if (_currentHealth < quarter * 2 && _currentHealth >= quarter)
            OnPlayerSq?.Invoke(this, EventArgs.Empty);
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        IsMoving = Mathf.Abs(inputVector.x) > MinMovingSpeed || Mathf.Abs(inputVector.y) > MinMovingSpeed;
    }
    
    public bool isMoving()
    {
        return IsMoving;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
}