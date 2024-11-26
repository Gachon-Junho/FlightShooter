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
        shootCoroutine = StartCoroutine(startShootLoop());
    }

    private void onBecameInvisible()
    {
        StopCoroutine(followCoroutine);
        StopCoroutine(shootCoroutine);
        
        HP.DecreaseHP(int.MaxValue);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
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
    
            yield return new WaitForFixedUpdate();
            
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