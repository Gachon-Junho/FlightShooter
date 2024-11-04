using UnityEngine;

public abstract class AttackableAircraft : Aircraft
{
    [SerializeField] 
    protected GameObject Projectile;
    
    public abstract double ShootInterval { get; }

    public virtual void Shoot()
    {
        
    }
}