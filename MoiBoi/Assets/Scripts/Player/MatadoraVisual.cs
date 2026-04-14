using UnityEngine;

public class MatadoraVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool("IsRunning", Matadora.Instance.isMoving());
        Flip();
    }

    private void Flip()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 PlayerPosition = Matadora.Instance.GetPlayerScreenPosition();

        if (mousePos.x < PlayerPosition.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }  
    }
    

}
