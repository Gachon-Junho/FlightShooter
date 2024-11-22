using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAircraft : AttackableAircraft
{
    private Coroutine shootLoop;

    private void Start()
    {
        HP.OnDead += () => GameplayManager.Current.EndGame(GameplayManager.GameResultState.Fail);
    }

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

        var dirVector = new Vector2(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180)).normalized;
        spawner.Direction = dirVector;
    }

    private IEnumerator startShootLoop()
    {
        Shoot();

        yield return new WaitForSeconds(ShootInterval);

        shootLoop = StartCoroutine(startShootLoop());
    }
}