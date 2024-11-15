using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAircraft : AttackableAircraft
{
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        Shoot();
    }
}