﻿using System;
using UnityEngine;

public class ProjectileSpawner : ObjectSpawner
{
    public ProjectileInfo ProjectileInfo;
    
    private ProjectilePool projectilePool => ProjectilePool.Current;

    public Vector2 Direction;
    
    public override GameObject SpawnObject(Action<GameObject> setupAction = null)
    {
        var obj = projectilePool.Get(p => p.ProjectileType == ProjectileInfo.ProjectileType,
            setupPosition, SpawnTarget);
        
        setupAction?.Invoke(obj.gameObject);

        return obj.gameObject;
    }

    private void setupPosition(Projectile projectile)
    {
        projectile.Initialize(ProjectileInfo, Direction, ProjectileInfo.Speed, GameplayManager.Current.Stage);
        projectile.transform.position = transform.position;
    }
}