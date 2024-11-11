using System;
using Object.Projectiles;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : PoolableGameObject
{
    public AttackableAircraft Owner;
    public virtual float Speed { get; private set; }
    public float Damage { get; set; }

    public virtual void Initialize(ProjectileInfo info)
    {
        Speed = info.Speed;
        Damage = info.Damage;
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