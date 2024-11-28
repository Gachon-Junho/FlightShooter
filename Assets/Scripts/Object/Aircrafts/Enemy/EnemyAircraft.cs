using System.Collections;
using UnityEngine;

public class EnemyAircraft : AttackableAircraft, IFollowingObject
{
    private PlayerAircraft player => GameplayManager.Current.Player;

    private Coroutine followCoroutine;
    private Coroutine shootCoroutine;

    private void Start()
    {
        HP.OnDead += () =>
        {
            GameplayManager.Current.Stage.DownedAircraftCount++;
            Destroy(gameObject);
        };

        // 이 기체의 메인 카메라부터의 가시성 여부를 판단해 처리합니다.
        StartCoroutine(this.CheckVisibility(Camera.main, 0.1f, () => onBecameVisible(), () => onBecameInvisible()));
        
        followCoroutine = StartCoroutine(follow());
        shootCoroutine = StartCoroutine(startShootLoop());
    }
    
    public override void Shoot()
    {
        foreach (var spawner in ProjectileSpawnPoints)
        {
            spawner.SpawnObject(go =>
            {
                var guidedMissile = go.GetComponent<GuidedMissile>();
    
                if (guidedMissile == null)
                    return;
    
                // 적 기체의 목표는 오직 플레이어 뿐입니다.
                guidedMissile.Target = GameplayManager.Current.Player.gameObject;
            });
        }
    }

    public void FollowTo(GameObject obj, float speed)
    {
        var dirVector = new Vector2(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
        var destination = new Vector3(obj.transform.position.x + dirVector.x, transform.position.y + dirVector.y, 0);
        
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }
    
    private void onBecameVisible()
    {
        // 화면 안에 보이기 시작하면 발사 루프를 시작합니다.
        shootCoroutine = StartCoroutine(startShootLoop());
    }

    private void onBecameInvisible()
    {
        // 더이상 보이지 않게되면 사망한 것으로 처리힙니다.
        
        StopCoroutine(followCoroutine);
        StopCoroutine(shootCoroutine);
        
        HP.DecreaseHP(int.MaxValue);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 카미카제 구현.
        var player = other.gameObject.GetComponent<PlayerAircraft>();
            
        if (player == null)
            return;

        player.HP.DecreaseHP(int.MaxValue);
        HP.DecreaseHP(int.MaxValue);
    }

    private IEnumerator follow()
    {
        if (player != null)
        {
            FollowTo(player.gameObject, Speed);
    
            yield return null;
            
            followCoroutine = StartCoroutine(follow());
        }
    }

    private IEnumerator startShootLoop()
    {
        Shoot();

        yield return new WaitForSeconds(ShootInterval);

        shootCoroutine = StartCoroutine(startShootLoop());
    }
}