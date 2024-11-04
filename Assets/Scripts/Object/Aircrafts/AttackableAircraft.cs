using UnityEngine;

public abstract class AttackableAircraft : Aircraft
{
    [SerializeField] 
    protected Projectile Projectile;
    
    public double ShootInterval { get; private set; }

    public override void Initialize(AircraftInfo info)
    {
        base.Initialize(info);

        var attackable = info as AttackableAircraftInfo;
        
        Debug.Assert(attackable != null);

        Projectile = attackable.Projectile;
        ShootInterval = attackable.ShootInterval;
    }

    public virtual void Shoot()
    {
        
    }
}