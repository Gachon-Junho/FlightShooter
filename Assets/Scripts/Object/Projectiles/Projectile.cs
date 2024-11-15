using System.Collections;
using UnityEngine;

public class Projectile : PoolableGameObject, IMovableObject
{
    public AttackableAircraft Owner;
    public virtual float Speed { get; private set; }
    public float Damage { get; set; }

    public virtual void Initialize(ProjectileInfo info, StageData stage = null)
    {
        Speed = info.Speed;
        Damage = info.Damage;
        
        if (stage != null)
             Damage *= stage.DamageWeight;
    }
    
    public IEnumerator MoveTo(Vector3 direction, float speed)
    {
        throw new System.NotImplementedException();
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