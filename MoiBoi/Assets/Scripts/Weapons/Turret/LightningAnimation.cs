using UnityEngine;

public class LightningAnimation : MonoBehaviour
{
    public Animator animator;
    public float changeInterval = 0.3f; // скорость смены анимаций
    
    private float _timer;
    private int _state = 0;
    private string[] _animations = { "WeakHit", "MediumHit", "StrongHit" };
    
    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        // Запускаем первую анимацию
        PlayAnimation(0);
    }
    
    void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= changeInterval)
        {
            _timer = 0f;
            _state = (_state + 1) % 3;
            PlayAnimation(_state);
        }
    }
    
    void PlayAnimation(int index)
    {
        // Принудительно запускаем анимацию с начала
        animator.Play(_animations[index], 0, 0f);
    }
}