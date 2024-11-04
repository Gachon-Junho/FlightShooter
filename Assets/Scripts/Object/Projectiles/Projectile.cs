using System;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IPoolableGameObject
{
    public IObjectPool<GameObject> Pool { get; set; }

    public AttackableAircraft Owner;
    public virtual float Speed { get; }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        throw new NotImplementedException();
    }

    public void Release()
    {
        Pool.Release(gameObject);
    }
}