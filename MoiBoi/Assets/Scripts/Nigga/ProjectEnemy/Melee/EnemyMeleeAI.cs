using UnityEngine;

public class EnemyMeleeAI : EnemyAIBase
{
    protected override void ChasingBehaviour()
    {
        _navMeshAgent.SetDestination(GetTargetPoint());
    }

    protected override void PerformAttack()
    {
        // Пусто! Урон через OnTriggerStay2D в EnemyMeleeEntity
    }
}