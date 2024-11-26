using System;
using UnityEngine;

public class GuidedMissile : Projectile, IFollowingObject
{
    public override ProjectileType ProjectileType => ProjectileType.GuidedMissile;

    public const float TRACK_SPEED = 50f;

    public GameObject Target;

    protected override void Update()
    {
        base.Update();

        if (gameObject.activeSelf)
        {
            FollowTo(Target, Speed);
        }
    }

    public void FollowTo(GameObject obj, float speed)
    {
        var dir = Direction;
        
        if (obj == null)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Aircraft");
            float shortestDistance = Mathf.Infinity;
            
            foreach (var aircraft in taggedObjects)
            {
                if (aircraft.GetComponent<PlayerAircraft>() != null) continue;
                
                float distance = Vector3.Distance(transform.position, aircraft.transform.position);
                
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    obj = Target = aircraft;
                    dir = (obj.transform.position - transform.position).normalized;
                }
            }
        }
        
        float angle = Vector2.Angle(transform.up, dir);

        if (angle <= 30)
        {
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, TRACK_SPEED * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        
        transform.position += transform.up * (speed * Time.deltaTime);
    }
}