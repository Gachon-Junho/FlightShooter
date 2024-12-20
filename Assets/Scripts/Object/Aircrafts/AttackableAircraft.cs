using UnityEngine;

public abstract class AttackableAircraft : Aircraft
{
    [SerializeField] 
    protected ProjectileInfo[] ProjectileInfo;
    
    public float ShootInterval { get; private set; }

    /// <summary>
    /// 공격 가능한 기체의 발사체 발사 지점들.
    /// </summary>
    public ProjectileSpawner[] ProjectileSpawnPoints => projectileSpawnPoints;

    [SerializeField]
    private ProjectileSpawner[] projectileSpawnPoints;

    public override void Initialize(AircraftInfo info, StageData stage = null)
    {
        base.Initialize(info, stage);

        var attackable = info as AttackableAircraftInfo;
        
        Debug.Assert(attackable != null);

        ProjectileInfo = attackable.ProjectileInfo;
        ShootInterval = attackable.ShootInterval;

        for (int i = 0; i < ProjectileSpawnPoints.Length; i++)
        {
            initializeSpawner(ProjectileSpawnPoints[i], ProjectileInfo[i]);
        }
    }

    protected virtual void initializeSpawner(ProjectileSpawner spawner, ProjectileInfo info)
    {
        spawner.SpawnTarget = info.TargetPrefab;
        spawner.ProjectileInfo = info;
        spawner.Owner = this;
    }

    public virtual void Shoot()
    {
        foreach (var spawner in ProjectileSpawnPoints)
        {
            spawner.SpawnObject();
        }
    }
}