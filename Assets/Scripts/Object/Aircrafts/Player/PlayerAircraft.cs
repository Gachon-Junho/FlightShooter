using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAircraft : AttackableAircraft
{
    private Coroutine shootLoop;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            shootLoop = StartCoroutine(startShootLoop());
        else if (context.canceled)
            StopCoroutine(shootLoop);
    }

    protected override void initializeSpawner(ProjectileSpawner spawner, ProjectileInfo info)
    {
        base.initializeSpawner(spawner, info);

        spawner.Direction = new Vector2(0, 1);
    }

    private IEnumerator startShootLoop()
    {
        Shoot();

        yield return new WaitForSeconds(ShootInterval);

        shootLoop = StartCoroutine(startShootLoop());
    }
}