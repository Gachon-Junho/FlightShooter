using System.Collections;
using UnityEngine;

public class EnemyAircraft : AttackableAircraft, IFollowingObject
{
    private PlayerAircraft player => GameplayManager.Current.Player;

    private Coroutine currentCoroutine;

    private void Start()
    {
        HP.OnDead += () =>
        {
            GameplayManager.Current.Stage.DownedAircraftCount++;
            Destroy(gameObject);
        };
    }

    private void OnEnable() => currentCoroutine = StartCoroutine(follow());

    public void FollowTo(GameObject obj, float speed)
    {
        var dirVector = new Vector2(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.PI / 180));
        var destination = new Vector3(obj.transform.position.x + dirVector.x, transform.position.y + dirVector.y, 0);
        
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        GameplayManager.Current.Stage.DownedAircraftCount++;
        StopCoroutine(currentCoroutine);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerAircraft>();
            
        if (player == null)
            return;

        player.HP.DecreaseHP(int.MaxValue);
    }

    private IEnumerator follow()
    {
        FollowTo(player.gameObject, Speed);

        yield return new WaitForFixedUpdate();

        currentCoroutine = StartCoroutine(follow());
    }
}