using UnityEngine;

public class EnemyVisualProject : MonoBehaviour
{
    [SerializeField] private EnemyAIProject _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;

    
    private Animator _animator;
    private const string ISRUNNING = "IsRunning";
    private const string CHASINGSPEEDMULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKEHIT = "TakeHit";
    private const string DEATH = "IsDead";

    SpriteRenderer _spriteRenderer;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakingHit += _enemyEntity_OnTakingHit;
        _enemyEntity.Death += _enemyEntity_Death;
    }

    private void _enemyEntity_Death(object sender, System.EventArgs e)
    {
        _animator.SetBool(DEATH, true);
        _spriteRenderer.sortingOrder = -1;
    }

    private void _enemyEntity_OnTakingHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKEHIT);
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }

    private void Update()
    {
        _animator.SetBool(ISRUNNING, _enemyAI.isRunning());
        _animator.SetFloat(CHASINGSPEEDMULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    public void EnemyDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
