using System.Runtime.InteropServices;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{

    public static ActiveWeapon Instance { get; private set; }

    [SerializeField] private Sword sword;

    private void Awake()
    {
        Instance = this;
    }
    public Sword getActiveWeapon()
    {
        return sword;
    }

    private void Update()
    {
        SwordFlip();
    }
    private void SwordFlip()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 PlayerPosition = Matadora.Instance.GetPlayerScreenPosition();

        if (mousePos.x < PlayerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }

}
