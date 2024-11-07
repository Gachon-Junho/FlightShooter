using System;
using Object.Projectiles;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IPoolableGameObject
{
    public IObjectPool<GameObject> Pool { get; set; }

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
        // Todo: IHasHitPoint를 가지고 있는 오브젝트인지 확인이 필요함. 태그로 확인하는 것도 가능하지만 지양해야할 것임.
        throw new NotImplementedException();
    }

    public void Release()
    {
        Pool.Release(gameObject);
    }
}