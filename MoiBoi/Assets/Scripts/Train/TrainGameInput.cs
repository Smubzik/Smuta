using UnityEngine;
using UnityEngine.InputSystem;

public class TrainGameInput : MonoBehaviour
{
    public static TrainGameInput Instance { get; private set; }

    private TrainInputActions trainInputActions;

    private void Awake()
    {
        Instance = this;

        trainInputActions = new TrainInputActions();
        trainInputActions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = trainInputActions.Train.Movement.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
};
