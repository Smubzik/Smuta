using UnityEngine;

public class TrainVisual : MonoBehaviour
{
    private Animator _animator;
    private Train train;
    
    private const string IS_fq = "Is50-75";
    private const string IS_sq = "Is25-49";
    private const string IS_tq = "IsDead";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        train = Train.Instance;
        
        if (train != null)
        {
            train.OnPlayerDeath += Player_OnPlayerDeath;
            train.OnPlayerFq += Player_OnPlayerFq;
            train.OnPlayerSq += Player_OnPlayerSq;
        }
        
        // Восстанавливаем анимацию через 0.1 секунды (ждём полной загрузки)
        Invoke(nameof(RestoreAnimation), 0.1f);
    }
    
    private void OnEnable()
    {
        // При возврате из меню (объект снова активен)
        Invoke(nameof(RestoreAnimation), 0.05f);
    }
    
    private void RestoreAnimation()
    {
        if (train == null)
        {
            train = Train.Instance;
            if (train == null) return;
        }
        
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            if (_animator == null) return;
        }
        
        // Сбрасываем все параметры
        _animator.Rebind();
        
        int currentHealth = train.CurrentHealth;
        int maxHealth = train.MaxHealth;
        
        if (maxHealth <= 0) return;
        
        float healthPercent = (float)currentHealth / maxHealth;
        
        Debug.Log($"Восстановление анимации: HP={currentHealth}/{maxHealth}, {healthPercent*100}%");
        
        // Устанавливаем правильный параметр
        if (currentHealth <= 0)
        {
            _animator.SetBool(IS_tq, true);
        }
        else if (healthPercent < 0.5f)
        {
            _animator.SetBool(IS_sq, true);
        }
        else if (healthPercent < 0.75f)
        {
            _animator.SetBool(IS_fq, true);
        }
        
        // Принудительно обновляем аниматор
        _animator.Update(0f);
    }
    
    private void Update()
    {
        // Каждый кадр проверяем и обновляем анимацию при изменении здоровья
        if (train != null && _animator != null)
        {
            // Проверяем, не нужно ли обновить анимацию
            int currentHealth = train.CurrentHealth;
            int maxHealth = train.MaxHealth;
            
            if (maxHealth <= 0) return;
            
            float healthPercent = (float)currentHealth / maxHealth;
            
            bool shouldBeFq = (healthPercent >= 0.5f && healthPercent < 0.75f);
            bool shouldBeSq = (healthPercent >= 0.25f && healthPercent < 0.5f);
            bool shouldBeDead = (currentHealth <= 0);
            
            // Проверяем, правильные ли сейчас параметры
            bool isFq = _animator.GetBool(IS_fq);
            bool isSq = _animator.GetBool(IS_sq);
            bool isDead = _animator.GetBool(IS_tq);
            
            if (shouldBeFq != isFq || shouldBeSq != isSq || shouldBeDead != isDead)
            {
                // Если параметры не совпадают — восстанавливаем
                Debug.Log("Анимация рассинхронизирована, восстанавливаем...");
                RestoreAnimation();
            }
        }
    }

    private void Player_OnPlayerSq(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_sq, true);
        _animator.SetBool(IS_fq, false);
        _animator.SetBool(IS_tq, false);
    }

    private void Player_OnPlayerFq(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_fq, true);
        _animator.SetBool(IS_sq, false);
        _animator.SetBool(IS_tq, false);
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_tq, true);
        _animator.SetBool(IS_fq, false);
        _animator.SetBool(IS_sq, false);
    }
}