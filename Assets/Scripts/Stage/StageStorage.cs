using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StageStorage", menuName = "Stage/Stage Storage", order = 0)]
public class StageStorage : ScriptableObject
{
    public StageData[] StageData => stageData;

    [SerializeField]
    private StageData[] stageData;
}

[Serializable]
public class StageData : IDeepCloneable<StageData>
{
    public float HPWeight;
    public float DamageWeight;
    public AircraftSetting[] AircraftSetting;

    public float MinSpawnInterval;
    public float MaxSpawnInterval;

    public bool IsCleared => AircraftSetting.Where(a => a.Amount != 0) == null;
    
    public StageData DeepClone()
    {
        return new StageData
        {
            HPWeight = HPWeight,
            DamageWeight = DamageWeight,
            AircraftSetting = AircraftSetting.Select(a => a.DeepClone()).ToArray(),
            MinSpawnInterval = MinSpawnInterval,
            MaxSpawnInterval = MaxSpawnInterval
        };
    }
}

[Serializable]
 public class AircraftSetting : IDeepCloneable<AircraftSetting>
 {
     public AttackableAircraftInfo AircraftInfo;
     public int Amount;


     public AircraftSetting DeepClone()
     {
         return new AircraftSetting
         {
             AircraftInfo = ((IDeepCloneable<AttackableAircraftInfo>)AircraftInfo).DeepClone(),
             Amount = Amount
         };
     }
 }