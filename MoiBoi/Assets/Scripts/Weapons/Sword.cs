using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwordSwing;
    public void Attack()
    {
        Debug.Log("Attack() called from: " + StackTraceUtility.ExtractStackTrace());
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }
}
