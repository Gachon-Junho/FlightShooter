using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AircraftName", menuName = "Aircraft/Attackable Aircraft Info")]
public class AttackableAircraftInfo : AircraftInfo, IDeepCloneable<AttackableAircraftInfo>
{
    public ProjectileInfo[] ProjectileInfo => projectileInfo;
    public float ShootInterval => shootInterval;
    
    [SerializeField] 
    private ProjectileInfo[] projectileInfo;

    [SerializeField]
    private float shootInterval;

    public AttackableAircraftInfo(string aircraftName, float speed, float hp, GameObject targetPrefab, ProjectileInfo[] projectileInfo, float shootInterval)
        : base(aircraftName, speed, hp, targetPrefab)
    {
        this.projectileInfo = projectileInfo;
        this.shootInterval = shootInterval;
    }

    public override AircraftInfo DeepClone()
    {
        throw new NotImplementedException();
    }

    AttackableAircraftInfo IDeepCloneable<AttackableAircraftInfo>.DeepClone()
    {
        return new AttackableAircraftInfo(Name, Speed, HP.MaxHP, TargetPrefab, ProjectileInfo.Select(p => p.DeepClone()).ToArray(), ShootInterval);
    }
}