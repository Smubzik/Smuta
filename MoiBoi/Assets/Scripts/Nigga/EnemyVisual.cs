using UnityEngine;



public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private enemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;

    private Animator _animator;
    private const string ISRUNNING = "IsRunning";
    private const string CHASINGSPEEDMULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";



    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
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
}
