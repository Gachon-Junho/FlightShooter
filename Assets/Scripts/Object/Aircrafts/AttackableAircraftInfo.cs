using UnityEngine;

[CreateAssetMenu(fileName = "AircraftName", menuName = "Aircraft/Attackable Aircraft Info")]
public class AttackableAircraftInfo : AircraftInfo
{
    public ProjectileInfo ProjectileInfo => projectileInfo;
    public double ShootInterval => shootInterval;
    
    [SerializeField] 
    private ProjectileInfo projectileInfo;

    [SerializeField]
    private double shootInterval;
}