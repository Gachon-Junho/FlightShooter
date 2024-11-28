using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// 공격 가능한 기체의 정보를 담는 요소입니다.
/// 공격에 관한 필수적인 속성을 포함하고 있습니다.
/// </summary>
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