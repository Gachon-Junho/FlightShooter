using System;
using UnityEngine;

public class GuidedMissile : Projectile, IFollowingObject
{
    public override ProjectileType ProjectileType => ProjectileType.GuidedMissile;

    public const float TRACK_SPEED = 3f;

    public GameObject Target;

    private Vector3 moveDirection;

    private void Start()
    {
        Target = GameplayManager.Current.Player.gameObject;
    }

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
        if (obj == null)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Aircraft");
            float shortestDistance = Mathf.Infinity;
            
            foreach (var aircraft in taggedObjects)
            {
                float distance = Vector3.Distance(transform.position, aircraft.transform.position);
                
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    obj = Target = aircraft;
                }
            }
        }
        
        var dir = (obj?.transform.position - transform.position)?.normalized;
        moveDirection = Vector3.RotateTowards(transform.forward, dir ?? Direction, TRACK_SPEED * Time.deltaTime, 0);

        transform.rotation = Quaternion.Euler(0, 0, Quaternion.LookRotation(moveDirection).z);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
}