using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Aircraft/Projectile Info")]
public class ProjectileInfo : ScriptableObject, IEquatable<ProjectileInfo>
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
    private int damage;

    [SerializeField] 
    private GameObject targetPrefab;

    public ProjectileInfo(float speed, int damage, GameObject targetPrefab)
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
}
