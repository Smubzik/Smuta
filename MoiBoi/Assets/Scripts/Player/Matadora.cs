using System;
using Unity.VisualScripting;
using UnityEngine;

public class Matadora : MonoBehaviour
{

    public static Matadora Instance { get; private set; }
    [SerializeField] private float movingSpeed = 10f;
    Vector2 inputVector;


    private Rigidbody2D rb;

    private float MinMovingSpeed = 0.1f;
    private bool IsMoving = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        HandleMovement();
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
