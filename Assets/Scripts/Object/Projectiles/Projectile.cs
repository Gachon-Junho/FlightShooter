using System;
using System.Collections;
using UnityEngine;

public class Projectile : PoolableGameObject, IMovableObject
{
    public virtual ProjectileType ProjectileType { get; }
    public AttackableAircraft Owner;
    public virtual float Speed { get; private set; }
    public float Damage { get; set; }

    protected Vector2 Direction;

    public virtual void Initialize(ProjectileInfo info, Vector2 direction, float speed, StageData stage = null)
    {
        Speed = info.Speed;
        Damage = info.Damage;
        Direction = direction;
        
        if (stage != null)
             Damage *= stage.DamageWeight;
    }

    protected override void Update()
    {
        base.Update();

        if (gameObject.activeSelf)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)Direction.normalized, Speed * Time.deltaTime);

            var cameraPos = Camera.main!.WorldToViewportPoint(transform.position);
            
            if (cameraPos.y > 1 || cameraPos.y < -1 || cameraPos.x > 1 || cameraPos.x < -1)
                Return();
        }
    }

    public IEnumerator MoveTo(Vector3 direction, float speed)
    {
        throw new NotImplementedException();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        IHasHitPoint obj = other.gameObject.GetComponent<IHasHitPoint>();
        
        if (obj == null)
            return;
        
        obj.HP.DecreaseHP(Damage);
        
        Return();
    }
}