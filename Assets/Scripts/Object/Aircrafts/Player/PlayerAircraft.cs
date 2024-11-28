using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAircraft : AttackableAircraft
{
    private Coroutine shootLoop;

    private void Start()
    {
        HP.OnDead += () => onDead();
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

    private void onDead()
    {
        GameplayManager.Current.EndGame(GameplayManager.GameResultState.Fail);
        gameObject.SetActive(false);
    }

    protected override void initializeSpawner(ProjectileSpawner spawner, ProjectileInfo info)
    {
        base.initializeSpawner(spawner, info);

        var dirVector = new Vector2(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)).normalized;
        spawner.Direction = dirVector;
    }

    /// <summary>
    /// 발사체 발사 루프를 시작합니다.
    /// </summary>
    /// <remarks>
    /// 루프를 중단하기 위해 코루틴을 사용해 중지해야합니다.
    /// </remarks>
    private IEnumerator startShootLoop()
    {
        Shoot();

        yield return new WaitForSeconds(ShootInterval);

        shootLoop = StartCoroutine(startShootLoop());
    }
}