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
            
            // 카메라에 자기자신이 있는지 확인 후 반환여부 결정
            if (!this.IsVisibleInCamera(Camera.main))
                onBecameInvisible();
        }
    }
    
    private void onBecameInvisible()
    {
        if (IsInUse)
            Return();
    }

    public IEnumerator MoveTo(Vector3 direction, float speed)
    {
        throw new NotImplementedException();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 HP를 가지고 있는 오브젝트인지 확인
        IHasHitPoint obj = other.gameObject.GetComponent<IHasHitPoint>();
        
        // HP 요소가 없거나 자기자신인 경우는 건너뜀
        if (obj == null || ReferenceEquals(Owner.HP, obj.HP))
            return;
        
        obj.HP.DecreaseHP(Damage);
        
        Return();
    }
}