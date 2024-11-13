using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAircraft : AttackableAircraft
{
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log($"{context.ReadValue<Vector2>()}");
        MoveDirection = context.ReadValue<Vector2>();
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        Shoot();
    }
}