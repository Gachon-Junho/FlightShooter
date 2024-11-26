using UnityEngine;

public class GuidedMissile : Projectile, IFollowingObject
{
    public override ProjectileType ProjectileType => ProjectileType.GuidedMissile;

    public const float TRACK_SPEED = 3f;

    private Vector3 moveDirection;

    protected override void Update()
    {
        base.Update();

        if (gameObject.activeSelf)
        {
            FollowTo(GameplayManager.Current.Player.gameObject, Speed);
        }
    }

    public void FollowTo(GameObject obj, float speed)
    {
        var dir = (obj.transform.position - transform.position).normalized;
        moveDirection = Vector3.RotateTowards(transform.forward, dir, TRACK_SPEED * Time.deltaTime, 0);

        transform.rotation = Quaternion.LookRotation(moveDirection);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
}