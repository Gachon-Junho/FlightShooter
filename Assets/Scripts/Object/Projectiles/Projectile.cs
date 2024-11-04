using System;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IPoolableGameObject
{
    public IObjectPool<GameObject> Pool { get; set; }

    public AttackableAircraft Owner;
    public virtual double Speed { get; }

    protected void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    public void Release()
    {
        Pool.Release(gameObject);
    }
}