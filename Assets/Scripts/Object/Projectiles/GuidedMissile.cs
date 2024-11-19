using UnityEngine;

public class GuidedMissile : Projectile, IFollowingObject
{
    public override ProjectileType ProjectileType => ProjectileType.GuidedMissile;

    
    public void FollowTo(GameObject obj, double speed)
    {
        throw new System.NotImplementedException();
    }
}