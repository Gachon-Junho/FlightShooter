using UnityEngine;

public class ProjectileSpawner<T> : ObjectSpawner where T : Projectile
{
    [SerializeField]
    private ProjectileInfo projectileInfo;
    
    private ProjectilePool projectilePool => ProjectilePool.Current;
    
    public override GameObject SpawnObject()
    {
        var obj = projectilePool.Get<T>(setupPosition, SpawnTarget);

        return obj.gameObject;
    }

    private void setupPosition(T projectile)
    {
        projectile.Initialize(projectileInfo, GameplayManager.Current.Stage);
        projectile.transform.position = transform.position;
    }
}