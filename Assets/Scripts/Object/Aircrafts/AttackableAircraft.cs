using UnityEngine;

public abstract class AttackableAircraft : Aircraft
{
    [SerializeField] 
    protected ProjectileInfo[] ProjectileInfo;
    
    public double ShootInterval { get; private set; }

    [SerializeField]
    private ProjectileSpawner[] ProjectileSpawnPoints;

    public override void Initialize(AircraftInfo info, StageData stage = null)
    {
        base.Initialize(info, stage);

        var attackable = info as AttackableAircraftInfo;
        
        Debug.Assert(attackable != null);

        ProjectileInfo = attackable.ProjectileInfo;
        ShootInterval = attackable.ShootInterval;

        for (int i = 0; i < ProjectileSpawnPoints.Length; i++)
        {
            ProjectileSpawnPoints[i].SpawnTarget = ProjectileInfo[i].TargetPrefab;
            ProjectileSpawnPoints[i].ProjectileInfo = ProjectileInfo[i];
        }
    }

    public virtual void Shoot()
    {
        foreach (var spawner in ProjectileSpawnPoints)
        {
            spawner.SpawnObject();
        }
    }
}