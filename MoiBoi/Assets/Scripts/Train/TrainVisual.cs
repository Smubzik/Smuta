using UnityEngine;

public class TrainVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator _animator;
    private const string IS_fq = "Is50-75";
    private const string IS_sq = "Is25-49";
    private const string IS_tq = "IsDead";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Train.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Train.Instance.OnPlayerFq += Player_OnPlayerFq;
        Train.Instance.OnPlayerSq += Player_OnPlayerSq;
    }

    private void Player_OnPlayerSq(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_sq, true);
    }

    private void Player_OnPlayerFq(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_fq, true);
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_tq, true);
    }
}
