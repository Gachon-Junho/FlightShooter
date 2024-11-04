using UnityEngine;

public class EnemyAircraft : AttackableAircraft, IFollowingObject
{
    public override string Name { get; }
    public override double Speed { get; }
    public override double ShootInterval { get; }  
    public override int HP { get; }
    
    public void FollowTo(GameObject obj, double speed)
    {
        throw new System.NotImplementedException();
    }
}