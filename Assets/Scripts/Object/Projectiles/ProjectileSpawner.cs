using System;
using UnityEngine;

public class ProjectileSpawner : ObjectSpawner
{
    public ProjectileInfo ProjectileInfo;
    
    private ProjectilePool projectilePool => ProjectilePool.Current;

    /// <summary>
    /// 발사체가 향할 방향.
    /// </summary>
    public Vector2 Direction;
    
    /// <summary>
    /// 발사체를 생성한 기체.
    /// </summary>
    public AttackableAircraft Owner;
    
    public override GameObject SpawnObject(Action<GameObject> setupAction = null)
    {
        // 발사체 타입이 일치하는 오브젝트를 풀에서 꺼냄
        var obj = projectilePool.Get(p => p.ProjectileType == ProjectileInfo.ProjectileType, setup, SpawnTarget);
        
        setupAction?.Invoke(obj.gameObject);

        return obj.gameObject;
    }

    private void setup(Projectile projectile)
    {
        projectile.Initialize(ProjectileInfo, Direction, ProjectileInfo.Speed, GameplayManager.Current.Stage);
        projectile.transform.position = transform.position;
        projectile.Owner = Owner;
    }
}