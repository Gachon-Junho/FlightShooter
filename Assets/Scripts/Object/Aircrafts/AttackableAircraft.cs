using System;
using UnityEngine;

public abstract class AttackableAircraft : Aircraft
{
    [SerializeField] 
    protected Projectile Projectile;
    
    public double ShootInterval { get; private set; }

    public override void Initialize(AircraftInfo info, StageData stage = null)
    {
        base.Initialize(info, stage);

        var attackable = info as AttackableAircraftInfo;
        
        Debug.Assert(attackable != null);

        Projectile = attackable.Projectile;
        ShootInterval = attackable.ShootInterval;

        if (stage != null)
            Projectile.Damage *= stage.DamageWeight;
    }

    public virtual void Shoot()
    {
    }
}