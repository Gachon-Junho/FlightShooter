using UnityEngine;

[CreateAssetMenu(fileName = "AircraftName", menuName = "Aircraft/Attackable Aircraft Info")]
public class AttackableAircraftInfo : AircraftInfo
{
    public Projectile Projectile => projectile;
    public double ShootInterval => shootInterval;
    
    [SerializeField] 
    private Projectile projectile;

    [SerializeField]
    private double shootInterval;
}