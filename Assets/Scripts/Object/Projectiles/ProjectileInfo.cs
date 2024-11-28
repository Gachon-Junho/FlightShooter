using System;
using UnityEngine;

/// <summary>
/// 발사체에 대한 정보를 담는 요소입니다.
/// 발사체의 유형과 가져야 할 여러 값을 포함하고 있습니다.
/// </summary>
[CreateAssetMenu(fileName = "Projectile", menuName = "Aircraft/Projectile Info")]
public class ProjectileInfo : ScriptableObject, IEquatable<ProjectileInfo>, IDeepCloneable<ProjectileInfo>
{
    public ProjectileType ProjectileType => projectileType;
    public float Speed => speed;
    public float Damage => damage;

    public GameObject TargetPrefab => targetPrefab;

    [SerializeField] 
    private ProjectileType projectileType;
    
    [SerializeField] 
    private float speed;
    
    [SerializeField] 
    private float damage;

    [SerializeField] 
    private GameObject targetPrefab;

    public ProjectileInfo(float speed, float damage, GameObject targetPrefab)
    {
        this.speed = speed;
        this.damage = damage;
        this.targetPrefab = targetPrefab;
    }

    public bool Equals(ProjectileInfo other)
    {
        if (other)
            return false;

        return Speed == other.Speed &&
               Damage == other.Damage &&
               ProjectileType == other.ProjectileType &&
               TargetPrefab.Equals(targetPrefab);
    }

    public ProjectileInfo(ProjectileType projectileType, float speed, float damage, GameObject targetPrefab)
    {
        this.projectileType = projectileType;
        this.speed = speed;
        this.damage = damage;
        this.targetPrefab = targetPrefab;
    }

    public ProjectileInfo DeepClone()
    {
        return new ProjectileInfo(ProjectileType, Speed, Damage, TargetPrefab);
    }
}
