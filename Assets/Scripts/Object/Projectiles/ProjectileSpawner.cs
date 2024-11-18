using System;
using UnityEngine;

public class ProjectileSpawner : ObjectSpawner
{
    [SerializeField]
    private ProjectileInfo projectileInfo;
    
    private ProjectilePool projectilePool => ProjectilePool.Current;

    public Vector2 Direction;
    
    public override GameObject SpawnObject(Action<GameObject> setupAction = null)
    {
        var obj = projectilePool.Get(p => p.Speed == projectileInfo.Speed && p.Damage == projectileInfo.Damage,
            setupPosition, SpawnTarget);
        
        setupAction?.Invoke(obj.gameObject);

        return obj.gameObject;
    }

    private void setupPosition(Projectile projectile)
    {
        projectile.Initialize(projectileInfo, Direction, projectileInfo.Speed, GameplayManager.Current.Stage);
        projectile.transform.position = transform.position;
    }
}